using GraphQL.Server;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App;

public static class AppGqlExtensions
{
	public static void AddAppGraphQL(this IServiceCollection services)
	{
		services.AddGraphQL(options =>
			{
				options.EnableMetrics = true;
				options.ExposeExceptions = true;
			})
			.AddGraphTypes(typeof(AppSchema), ServiceLifetime.Scoped)
			.AddNewtonsoftJson();

		services.AddScoped<ISchema, AppSchema>();
		services.AddScoped<AppGraphQuery>();
		services.AddScoped<AppGraphMutation>();

		services.AddScoped<MovieGraphType>();
	}
}