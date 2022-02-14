// ReSharper disable once CheckNamespace

using Movies.Contracts;
using Movies.Grains;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppGrainsServiceCollectionExtensions
{
	public static void AddAppGrains(this IServiceCollection services) => services.AddScoped<IMovieGrain, MovieGrain>();

	public static IServiceCollection AddAppHotsGrains(this IServiceCollection services) => services;

	public static IServiceCollection AddAppLoLGrains(this IServiceCollection services) => services;
}