using Movies.Contracts.Entity;

namespace Movies.Aggregates.Film;

public interface IMovieRepository
{
	Task<IQueryable<MovieModel>> GetList(string genre);
	Task<bool> AddAsync(MovieModel entity);
	Task<bool> UpdateAsync(string id, MovieModel entity);
	Task<MovieModel> Get(string id);
}