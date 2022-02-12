using System.Collections.Generic;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{
		Task<MovieModel> Get(string id);
		Task<IEnumerable<MovieModel>> GetList(string genre);
		Task<IEnumerable<MovieModel>> GetRatedFilms();
		Task Set(string key, string name);
		Task Update(string key, string name);
	}
}
