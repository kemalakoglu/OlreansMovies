using Movies.Contracts.Entity;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts;

public interface IMovieGrain : IGrainWithStringKey
{
	Task<MovieModel> Get(string id);
	Task<IEnumerable<MovieModel>> GetList(string genre);
	Task<MovieModel> Update(string id, MovieModel entity);
	Task<MovieModel> Set(MovieModel entity);
}