using Microsoft.Extensions.Caching.Distributed;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Core.Constants;
using Newtonsoft.Json;
using Orleans;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.GrainClients;

public class MovieGrainClient : IMovieGrainClient
{
	private static ConnectionMultiplexer _connectionMultiplexer;
	private readonly IDatabase _cache;
	private readonly IDistributedCache _distributedCache;
	private readonly IGrainFactory _grainFactory;

	public MovieGrainClient(
		IGrainFactory grainFactory,
		IDistributedCache distributedCache
	)
	{
		_grainFactory = grainFactory;
		_distributedCache = distributedCache;
	}

	public Task Set(MovieModel entity)
	{
		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = grain.Set(entity);
		ClearCache();
		return result;
	}

	public Task Update(string id, MovieModel entity)
	{
		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = grain.Update(id, entity);
		ClearCache();
		return result;
	}

	public async Task<MovieModel> Get(string id)
	{
		if (_distributedCache.GetString(id) != null)
		{
			var film = _distributedCache.GetString(id);
			MovieModel response = JsonConvert.DeserializeObject<MovieModel>(film);
			return response;
		}
		else
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			var result = grain.Get(id).Result;

			var jsonObj = JsonConvert.SerializeObject(result);
			_distributedCache.SetString(id, jsonObj);
			return result;
		}
	}

	public async Task<IEnumerable<MovieModel>> GetList(string genre)
	{
		if (string.IsNullOrEmpty(genre) && _distributedCache.GetString(RedisStates.GetAll) != null)
		{
			var filmList = _distributedCache.GetString(RedisStates.GetAll);
			var response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
			return response;
		}

		if (!string.IsNullOrEmpty(genre) && _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre) != null)
		{
			var filmList = _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre);
			var response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
			return response;
		}

		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = await grain.GetList(genre);


		var jsonList = JsonConvert.SerializeObject(result);
		if (string.IsNullOrEmpty(genre))
			_distributedCache.SetString(RedisStates.GetAll, jsonList);
		else
			_distributedCache.SetString(RedisStates.GetByGenre + "_" + genre, jsonList);

		return result;
	}

	public async Task<IEnumerable<MovieModel>> GetRatedMovies()
	{
		if (_distributedCache.GetString(RedisStates.GetAll) != null)
		{
			var filmList = _distributedCache.GetString(RedisStates.GetAll);
			IEnumerable<MovieModel> response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList).OrderByDescending(x => x.rate).Take(5);
			return response;
		}
		else
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			var result = await grain.GetList(string.Empty);


			var jsonList = JsonConvert.SerializeObject(result);
			_distributedCache.SetString(RedisStates.GetAll, jsonList);
			return result.OrderByDescending(x => x.rate).Take(5);
		}
	}

	public void RedisCacheService()
	{
		var connection = "localhost:6001, allowAdmin=true";
		_connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
	}

	public void ClearCache()
	{
		RedisCacheService();
		var endpoints = _connectionMultiplexer.GetEndPoints(true);
		foreach (var endpoint in endpoints)
		{
			var server = _connectionMultiplexer.GetServer(endpoint);
			server.FlushAllDatabases();
		}
	}
}