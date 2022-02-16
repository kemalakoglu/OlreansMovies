using Microsoft.Extensions.Caching.Distributed;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Core;
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

	public async Task<MovieModel> Set(MovieModel entity)
	{
		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = await grain.Set(entity);
		ClearCache();
		return result;
	}

	public async Task<MovieModel> Update(string id, MovieModel entity)
	{
		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = await grain.Update(id, entity);
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
			var result = await grain.Get(id);

			var jsonObj = JsonConvert.SerializeObject(result);
			_distributedCache.SetString(id, jsonObj);
			return result;
		}
	}

	public async Task<IEnumerable<MovieModel>> GetList(string genre, string name, string key, string description, double rate)
	{
		if (StringExtensions.IsNullOrEmpty(genre) && StringExtensions.IsNullOrEmpty(name) 
		    && StringExtensions.IsNullOrEmpty(key) && StringExtensions.IsNullOrEmpty(genre) 
		    && StringExtensions.IsNullOrEmpty(description) && _distributedCache.GetString(RedisStates.GetAll) != null)
		{
			var filmList = _distributedCache.GetString(RedisStates.GetAll);
			var response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
			return response;
		}

		//if (!StringExtensions.IsNullOrEmpty(genre) && _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre) != null)
		//{
		//	var filmList = _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre);
		//	var response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
		//	return response;
		//}

		var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
		var result = await grain.GetList(genre, name, key, description, rate);


		var jsonList = JsonConvert.SerializeObject(result);
		if (StringExtensions.IsNullOrEmpty(genre))
			_distributedCache.SetString(RedisStates.GetAll, jsonList);
		//else
		//	_distributedCache.SetString(RedisStates.GetByGenre + "_" + genre, jsonList);

		return result.DistinctBy(x=>x._id);
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
			var result = await grain.GetList(string.Empty, string.Empty, String.Empty, String.Empty, 0);


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
		string refreshToken = _distributedCache.GetString("refreshToken");
		RedisCacheService();
		var endpoints = _connectionMultiplexer.GetEndPoints(true);
		foreach (var endpoint in endpoints)
		{
			var server = _connectionMultiplexer.GetServer(endpoint);
			server.FlushAllDatabases();
		}

		_distributedCache.SetString("refreshToken", refreshToken);
	}
}