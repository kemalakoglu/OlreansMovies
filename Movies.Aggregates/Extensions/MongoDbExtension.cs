using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts;

namespace Movies.Aggregates.Extensions;

public static class MongoDbExtension
{
	public static IServiceCollection AddMongoDbSettings(this IServiceCollection services,
		IConfiguration configuration) =>
		services.Configure<MongoDbSettings>(options =>
		{
			options.ConnectionString = configuration
				.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.ConnectionStringValue).Value;
			options.Database = configuration
				.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.DatabaseValue).Value;
		});
}