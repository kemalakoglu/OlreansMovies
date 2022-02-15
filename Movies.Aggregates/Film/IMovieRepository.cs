using Movies.Contracts.Entity;

namespace Movies.Aggregates.Film;

public interface IMovieRepository
{
	Task<IQueryable<MovieModel>> GetList(string genre);
	Task<MovieModel> AddAsync(MovieModel entity);
	Task<MovieModel> UpdateAsync(string id, MovieModel entity);
	Task<MovieModel> Get(string id);
}