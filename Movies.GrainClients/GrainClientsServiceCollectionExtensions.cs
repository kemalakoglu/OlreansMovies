using Microsoft.Extensions.DependencyInjection;
using Movies.Aggregates.Film;
using Movies.Contracts;

namespace Movies.GrainClients;

public static class GrainClientsServiceCollectionExtensions
{
	public static void AddAppClients(this IServiceCollection services)
	{
		services.AddScoped<IMovieRepository, MovieRepository>();
		services.AddScoped<IMovieGrainClient, MovieGrainClient>();
	}
}