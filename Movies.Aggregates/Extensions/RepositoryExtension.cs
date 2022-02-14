using Microsoft.Extensions.DependencyInjection;
using Movies.Aggregates.Film;

namespace Movies.Aggregates.Extensions;

public static class RepositoryExtension
{
	public static void AddRepositories(this IServiceCollection services) =>
		services.AddScoped<IMovieRepository, MovieRepository>();
}