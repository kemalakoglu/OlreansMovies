using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<MovieModel> Get();
		Task Set(string name);
	}
}