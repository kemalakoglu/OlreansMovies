using Movies.Contracts.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts;

public interface IMovieGrainClient
{
	Task<MovieModel> Get(string id);
	Task<IEnumerable<MovieModel>> GetList(string genre, string name, string key, string description, double rate);
	Task<IEnumerable<MovieModel>> GetRatedMovies();
	Task<MovieModel> Set(MovieModel entity);
	Task<MovieModel> Update(string id, MovieModel entity);
}