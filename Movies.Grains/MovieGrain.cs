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
		var response = await _repository.Get(id);
		return response;
	}

	public async Task<IEnumerable<MovieModel>> GetList(string genre)
	{
		_repository = new MovieRepository();
		IEnumerable<MovieModel> response = await _repository.GetList(genre);

		return response;
	}

	public Task Set(MovieModel entity)
	{
		_repository = new MovieRepository();
		Task.FromResult(_repository.AddAsync(entity));
		return Task.CompletedTask;
	}

	public Task Update(string id, MovieModel entity)
	{
		_repository = new MovieRepository();
		Task.FromResult(_repository.UpdateAsync(id, entity));
		return Task.CompletedTask;
	}
}