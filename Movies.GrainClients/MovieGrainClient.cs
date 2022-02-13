using Microsoft.Extensions.Caching.Distributed;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Core.Constants;
using Newtonsoft.Json;
using Orleans;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;
		private readonly IDistributedCache _distributedCache;
		private readonly IDatabase _cache;
		private static ConnectionMultiplexer _connectionMultiplexer;

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
			Task result = grain.Set(entity);
			ClearCache();
			return result;
		}
		public Task Update(string id, MovieModel entity)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			Task result = grain.Update(id, entity);
			ClearCache();
			return result;
		}
		public Task<MovieModel> Get(string id)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			return grain.Get(id);
		}
		public async Task<IEnumerable<MovieModel>> GetList(string genre)
		{
			if(string.IsNullOrEmpty(genre) && _distributedCache.GetString(RedisStates.GetAll)!= null)
			{
				string filmList = _distributedCache.GetString(RedisStates.GetAll);
				IEnumerable<MovieModel> response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
				return response;
			}
			else if (!string.IsNullOrEmpty(genre) && _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre) != null)
			{
				string filmList = _distributedCache.GetString(RedisStates.GetByGenre + "_" + genre);
				IEnumerable<MovieModel> response = JsonConvert.DeserializeObject<IEnumerable<MovieModel>>(filmList);
				return response;
			}

			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			IEnumerable<MovieModel> result = await grain.GetList(genre);


			string jsonList = JsonConvert.SerializeObject(result).ToString();
			if (string.IsNullOrEmpty(genre))
				_distributedCache.SetString(RedisStates.GetAll, jsonList);
			else
				_distributedCache.SetString(RedisStates.GetByGenre + "_" + genre, jsonList);

			return result;
		}

		public async Task<IEnumerable<MovieModel>> GetRatedMovies()
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(Guid.NewGuid().ToString());
			return await grain.GetRatedMovies();
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
}