using Movies.Contracts;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public MovieGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public Task Set(string key, string name)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(key);
			return grain.Set(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Task Update(string key, string name) => throw new System.NotImplementedException();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<MovieModel> Get(string id)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.Get();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="genre"></param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Task<IEnumerable<MovieModel>> GetList(string genre) => throw new System.NotImplementedException();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Task<IEnumerable<MovieModel>> GetRatedFilms() => throw new System.NotImplementedException();
	}
}