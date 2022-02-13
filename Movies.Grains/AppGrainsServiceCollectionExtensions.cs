// ReSharper disable once CheckNamespace
using Microsoft.Extensions.Configuration;
using Movies.Aggregates.Film;
using Movies.Contracts;
using Movies.Grains;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class AppGrainsServiceCollectionExtensions
	{

		public static void AddAppGrains(this IServiceCollection services)
		{
			services.AddScoped<IMovieGrain, MovieGrain>();
		}

		public static IServiceCollection AddAppHotsGrains(this IServiceCollection services)
		{
			return services;
		}

		public static IServiceCollection AddAppLoLGrains(this IServiceCollection services)
		{
			return services;
		}
	}
}
