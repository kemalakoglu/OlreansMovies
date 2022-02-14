using Movies.Contracts.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts;

public interface IMovieGrainClient
{
	Task<MovieModel> Get(string id);
	Task<IEnumerable<MovieModel>> GetList(string genre);
	Task<IEnumerable<MovieModel>> GetRatedMovies();
	Task Set(MovieModel entity);
	Task Update(string id, MovieModel entity);
}