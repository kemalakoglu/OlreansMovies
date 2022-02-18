## OrleansMovies
OrleansMovies .Net 6.x support !

## IoC
ASP.NET Core Dependency <br/>

## Principles
SOLID <br/>
Domain Driven Design<br/>

## Persistance
MongoDb.Driver<br/>
###Requirements
When you run application on your local, have to run MongoDb on locally or Docker.
To run Docker, please run this command on your terminal "docker pull mongodb" then run this image with 27017 port if you start project debug mode.

## Cache
In-Memory
Redis

## Log
Serilog support
Elasticsearch

## Documentation
Swagger

## EventSourcing
Orleans

## Authentication
OAuth JWT Token
Dummy user credentials are;
Username: RiverTech
Password: Developer123!!
### Authentication Usage
You have to login with user cretentials then use access token to call API's on header authorization field without Bearer prefix. Each token has 5 min. When Access token was expired, you can get new access token with refreshToken data. It will allow you to get a new access token for another 5 minutes.
### Local Requirements
When you run application on your local, have to run Redis on locally or Docker
To run Docker, please run this command on your terminal "docker pull redis" then run this image with 6001 port if you start project debug mode.


## Benefits
 - Conventional Dependency Registering
 - Async await first 
 - GlobalQuery Filtering
 - Domain Driven Design Concepts
 - Repository pattern implementations
 - Object to object mapping with abstraction
 - .Net 6.x support
 - Aspect Oriented Programming
 - Simple Usage
 - Modularity
 - Event Sourcing
 
##API Documentation
Swagger

##Docker
docker build -t aspnetapp .
docker-compose up
 
   

***Basic Usage***

	public static Task Main(string[] args)
	{
		var hostBuilder = new HostBuilder();

		IAppInfo appInfo = null;
		hostBuilder
			.ConfigureHostConfiguration(cfg =>
			{
				cfg.SetBasePath(Directory.GetCurrentDirectory())
					.AddEnvironmentVariables("ASPNETCORE_")
					.AddCommandLine(args);
			})
			.ConfigureServices((ctx, services) =>
			{
				appInfo = new AppInfo(ctx.Configuration);
				Console.Title = $"{appInfo.Name} - {appInfo.Environment}";

				services.AddSingleton(appInfo);
				services.Configure<ApiHostedServiceOptions>(options =>
				{
					options.Port = GetAvailablePort(6600, 6699);
				});

				services.Configure<ConsoleLifetimeOptions>(options =>
				{
					options.SuppressStatusMessages = true;
				});
			})
			.ConfigureAppConfiguration((ctx, cfg) =>
			{
				var shortEnvName = AppInfo.MapEnvironmentName(ctx.HostingEnvironment.EnvironmentName);
				cfg.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.AddJsonFile($"appsettings.{shortEnvName}.json", true)
					.AddJsonFile("app-info.json")
					.AddEnvironmentVariables()
					.AddCommandLine(args);

				appInfo = new AppInfo(cfg.Build());

				if (!appInfo.IsDockerized) return;

				cfg.Sources.Clear();

				cfg.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.AddJsonFile($"appsettings.{shortEnvName}.json", true)
					.AddJsonFile("app-info.json")
					.AddEnvironmentVariables()
					.AddCommandLine(args);
			})
			//.UseSerilog((ctx, loggerConfig) =>
			//{
			//	loggerConfig.Enrich.FromLogContext()
			//		.ReadFrom.Configuration(ctx.Configuration)
			//		.Enrich.WithMachineName()
			//		.Enrich.WithDemystifiedStackTraces()
			//		.WriteTo.Console(
			//			outputTemplate:
			//			"[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}");

			//	loggerConfig.WithAppInfo(appInfo);
			//})
			.UseOrleans((ctx, builder) =>
			{
				builder
					.UseAppConfiguration(new AppSiloBuilderContext
					{
						AppInfo = appInfo,
						HostBuilderContext = ctx,
						SiloOptions = new AppSiloOptions
						{
							SiloPort = GetAvailablePort(11111, 12000), GatewayPort = 30001
						}
					})
					.ConfigureApplicationParts(parts => parts
						.AddApplicationPart(typeof(MovieGrain).Assembly).WithReferences()
					)
					.AddIncomingGrainCallFilter<LoggingIncomingCallFilter>()
					;
			})
			.ConfigureServices((ctx, services) =>
			{
				services.AddHostedService<ApiHostedService>();
			})
			;

		return hostBuilder.RunConsoleAsync();
	}
                         
					
					
***Conventional Registration***	 	

	public static void AddAppGrains(this IServiceCollection services) => services.AddScoped<IMovieGrain, MovieGrain>();

	public static IServiceCollection AddAppHotsGrains(this IServiceCollection services) => services;

	public static IServiceCollection AddAppLoLGrains(this IServiceCollection services) => services;
	
		public static void AddRepositories(this IServiceCollection services) =>
		services.AddScoped<IMovieRepository, MovieRepository>();
	
		public static void AddRepositories(this IServiceCollection services) =>
		services.AddScoped<IMovieRepository, MovieRepository>();
	 
***Swagger Activation***

	 services.ConfigureSwagger();
		app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/OrleansMovie/swagger.json", "OrleansMovie"); });


***Serilog Activation***

		
		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithProperty("Application", "app")
			.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
			.MinimumLevel.Override("System", LogEventLevel.Warning)
			//.WriteTo.File(new JsonFormatter(), "log.json")
			.ReadFrom.Configuration(configuration)
			.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("localhost:9200"))
			{
				AutoRegisterTemplate = true,
				AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
				FailureCallback = e => Console.WriteLine("fail message: " + e.MessageTemplate),
				EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
				                   EmitEventFailureHandling.WriteToFailureSink |
				                   EmitEventFailureHandling.RaiseCallback
				//FailureSink = new FileSink("log" + ".json", new JsonFormatter(), null)
			})
			.MinimumLevel.Verbose()
			.CreateLogger();
		Log.Information("WebApi Starting...");
		
		

***ErrorHandlingMiddleware Interceptor Activation***

     	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			Log.Write(LogEventLevel.Information, requestMessageTemplate,
				"Service path is:" + context.Request.Path.Value,
				context.Request.Body);
			await next(context);
		}
		catch (HttpRequestException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (AuthenticationException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (BusinessException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (UnauthorizedException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (Exception ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, object exception)
	{
		var code = HttpStatusCode.InternalServerError; // 500 if unexpected
		var message = string.Empty;
		var RC = string.Empty;
		var details = string.Empty;

		if (exception.GetType() == typeof(HttpRequestException))
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Failed;
			message = BusinessException.GetDescription(RC);
			details = ((HttpRequestException)exception).Message;
		}
		else if (exception.GetType() == typeof(AuthenticationException))
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Unauthorized;
			message = BusinessException.GetDescription(RC);
			details = ((AuthenticationException)exception).Message;
		}
		else if (exception.GetType() == typeof(BusinessException))
		{
			var businesException = (BusinessException)exception;
			message = businesException.Message;
			code = HttpStatusCode.OK;
			RC = businesException.RC;
			if (RC == ResponseCodes.ExpireToken || RC == ResponseCodes.InvalidToken || RC == ResponseCodes.Unauthorized)
				code = HttpStatusCode.Unauthorized;
			else if (StringExtensions.IsNullOrEmpty(businesException.RC))
				RC = ResponseCodes.Failed;
			else
				RC = businesException.RC;
			details = ((BusinessException)exception).Details;
		}
		else if (exception.GetType() == typeof(Exception))
		{
			code = HttpStatusCode.BadRequest;
			RC = ResponseCodes.BadRequest;
			message = BusinessException.GetDescription(RC);
			details = ((Exception)exception).Message;
		}
		else
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Failed;
			message = BusinessException.GetDescription(RC);
		}

		var response = new ErrorDTO
		{
			message = message, rc = RC, details = details, trackId = Guid.NewGuid().ToString()
		};
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)code;
		return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
	}


***Custom Repository definitions***
 

 public class MovieRepository : IMovieRepository
{
	protected readonly IMongoCollection<MovieModel> _collection;
	private readonly IMongoDbSettings mongoConfigDbSettings;
	public MovieRepository()
	{
		mongoConfigDbSettings = PrepareConfiguration();
		var dbClient = new MongoClient(mongoConfigDbSettings.ConnectionString);
		var db = dbClient.GetDatabase(mongoConfigDbSettings.DatabaseName);
		_collection = db.GetCollection<MovieModel>("Movies");
	}


	public async Task<MovieModel> AddAsync(MovieModel entity)
	{
		if (!StringExtensions.IsNullOrEmpty(entity._id))
			entity._id = null;
		var options = new InsertOneOptions { BypassDocumentValidation = false };
		await _collection.InsertOneAsync(entity, options);
		return entity;
	}

	public async Task<MovieModel> Get(string id) => _collection.Find(x => x._id == id).SingleOrDefault();

	public async Task<IQueryable<MovieModel>> GetList(string genre, string name, string key, string description, double rate)
	{
		var predicate = PredicateBuilder.True<MovieModel>();
		if (!StringExtensions.IsNullOrEmpty(genre))
			predicate = predicate.And(x => x.genres.Contains(genre));
		if (!StringExtensions.IsNullOrEmpty(name))
			predicate = predicate.And(x => x.name.Contains(name));
		if (!StringExtensions.IsNullOrEmpty(key))
			predicate = predicate.And(x => x.key.Contains(key));
		if (rate>0)
			predicate = predicate.And(x => x.rate >= rate);

		IEnumerable<MovieModel> filmList = _collection.Find(predicate).ToListAsync().Result;
		return filmList.AsQueryable();
	}

	public async Task<MovieModel> UpdateAsync(string id, MovieModel entity)
	{
		await _collection.FindOneAndReplaceAsync(x => x._id == id, entity);
		return entity;
	}

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

	private MongoDbSettings PrepareConfiguration()
	{
		if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
			return new MongoDbSettings(GetDevelopmentConfiguration());
		else
			return new MongoDbSettings(GetProductionConfiguration());
	}
}
     

