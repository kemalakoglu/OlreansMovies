using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Movies.Contracts.Entity;
using Movies.Core;
using Movies.Core.Utils;

namespace Movies.Aggregates.Film;

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