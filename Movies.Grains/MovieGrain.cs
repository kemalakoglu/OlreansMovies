using Movies.Aggregates.Film;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Grains;

[StorageProvider(ProviderName = "Default")]
public class MovieGrain : Grain<MovieModel>, IMovieGrain
{
	private IMovieRepository _repository;

	public async Task<MovieModel> Get(string id)
	{
		_repository = new MovieRepository();
		return await _repository.Get(id);
	}

	public async Task<IEnumerable<MovieModel>> GetList(string genre)
	{
		_repository = new MovieRepository();
		return await _repository.GetList(genre);
	}

	public async Task<MovieModel> Set(MovieModel entity)
	{
		_repository = new MovieRepository();
		return await _repository.AddAsync(entity);
	}

	public async Task<MovieModel> Update(string id, MovieModel entity)
	{
		_repository = new MovieRepository();
		return await _repository.UpdateAsync(id, entity);
	}
}