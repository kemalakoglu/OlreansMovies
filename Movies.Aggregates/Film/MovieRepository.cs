using MongoDB.Driver;
using Movies.Contracts.Entity;
using Movies.Core.Utils;

namespace Movies.Aggregates.Film;

public class MovieRepository : IMovieRepository
{
	protected readonly IMongoCollection<MovieModel> Collection;
	//private readonly MongoDbSettings settings;

	public MovieRepository()
	{
		var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
		var db = dbClient.GetDatabase("OrleansMovies");
		Collection = db.GetCollection<MovieModel>("Movies");
	}

	public async Task<MovieModel> AddAsync(MovieModel entity)
	{
		var options = new InsertOneOptions { BypassDocumentValidation = false };
		await Collection.InsertOneAsync(entity, options);
		return entity;
	}

	public async Task<MovieModel> Get(string id) => Collection.Find(x => x._id == id).SingleOrDefault();

	public async Task<IQueryable<MovieModel>> GetList(string genre)
	{
		var predicate = PredicateBuilder.True<MovieModel>();
		if (!string.IsNullOrEmpty(genre))
			predicate = predicate.And(x => x.genres.Contains(genre));

		IEnumerable<MovieModel> filmList = Collection.Find(predicate).ToListAsync().Result;
		return filmList.AsQueryable();
	}

	public async Task<MovieModel> UpdateAsync(string id, MovieModel entity)
	{
		await Collection.FindOneAndReplaceAsync(x => x._id == id, entity);
		return entity;
	}
}