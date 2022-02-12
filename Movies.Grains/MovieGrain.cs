using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<MovieModel>, IMovieGrain
	{
		public Task<MovieModel> Get()
			=> Task.FromResult(State);

		public Task Set(string name)
		{
			//State = new MovieModel(this.GetPrimaryKeyString(),name);
			return Task.CompletedTask;
		}
	}
}