using GraphiQl;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Aggregates.Extensions;
using Movies.Core;
using Movies.GrainClients;
using Movies.Server.Extensions;
using Movies.Server.Gql.App;
using Movies.Server.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace Movies.Server;

public class ApiStartup
{
	private readonly IAppInfo _appInfo;
	private readonly IConfiguration _configuration;

	public static IConfiguration GetProductionConfiguration()
	{
		return new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();
	}

	public static IConfiguration GetDevelopmentConfiguration()
	{
		return new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.dev.json", optional: true)
			.Build();
	}

	public ApiStartup(
		IConfiguration configuration,
		IAppInfo appInfo
	)
	{
		if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
			_configuration = GetDevelopmentConfiguration();
		else
			_configuration = GetProductionConfiguration();
		//_configuration = configuration;
		_appInfo = appInfo;
	}

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCustomAuthentication();
		services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
		{
			builder
				// .SetIsOriginAllowed((host) => true)
				.WithOrigins("http://localhost:4200")
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				;
		}));

		// note: to fix graphql for .net core 3
		services.Configure<KestrelServerOptions>(options =>
		{
			options.AllowSynchronousIO = true;
		});

		services.AddAppClients();
		services.AddAppGrains();
		services.AddAppGraphQL();
		services.AddControllers()
			.AddNewtonsoftJson();
		services.ConfigureLogger(_configuration);
		services.ConfigureSwagger();

		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = _configuration.GetSection("RedisCacheSettings:" + "host").Get<string>();
		});

		services.RunMongoDbSeeder(_configuration);
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(
		IApplicationBuilder app,
		IWebHostEnvironment env
	)
	{
		app.UseCors("TempCorsPolicy");

		// add http for Schema at default url /graphql
		app.UseGraphQL<ISchema>();

		// use graphql-playground at default url /ui/playground
		app.UseGraphQLPlayground();

		//app.UseGraphQLEndPoint<AppSchema>("/graphql");

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseGraphiQl();
		}

		// app.UseMiddleware(typeof(ErrorHandlingMiddleware));
		app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		app.UseStaticFiles();
		// Enable middleware to serve generated Swagger as a JSON endpoint.
		app.UseSwagger();
		// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
		//app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/ACPIdentity/swagger.json", "ACPIdentity"); });
		app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/OrleansMovie/swagger.json", "OrleansMovie"); });

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}