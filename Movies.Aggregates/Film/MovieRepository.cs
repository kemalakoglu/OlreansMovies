using MongoDB.Driver;
using Movies.Contracts.Entity;
using Movies.Core.Utils;
using System.Linq.Expressions;

namespace Movies.Aggregates.Film
{
	public class MovieRepository : IMovieRepository
	{
		protected readonly IMongoCollection<MovieModel> Collection;
		//private readonly MongoDbSettings settings;

		public MovieRepository()
		{
			var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
			IMongoDatabase db = dbClient.GetDatabase("OrleansMovies");
			this.Collection = db.GetCollection<MovieModel>("Movies");}

		public virtual async Task<bool> AddAsync(MovieModel entity)
		{
			var options = new InsertOneOptions { BypassDocumentValidation = false };
			await Collection.InsertOneAsync(entity, options);
			return true;
		}

		public async Task<MovieModel> Get(string id)
		{
			return Collection.Find(x=>x._id == id).SingleOrDefault();
		}

		public async Task<IQueryable<MovieModel>> GetList(string genre)
		{
			var predicate = PredicateBuilder.True<MovieModel>();
            if(!string.IsNullOrEmpty(genre))
			    predicate = predicate.And(x => x.genres.Contains(genre));

			IEnumerable<MovieModel> filmList = Collection.Find(predicate).ToListAsync().Result;
			return filmList.AsQueryable();

		}

		public async Task<IQueryable<MovieModel>> GetRatedMovies()
		{
			IEnumerable<MovieModel> filmList = Collection.Find(Builders<MovieModel>.Filter.Empty).ToListAsync().Result.OrderByDescending(x=>x.rate).Take(5);
			return filmList.AsQueryable();
		}

		public virtual async Task<bool> UpdateAsync(string id, MovieModel entity)
		{
			await Collection.FindOneAndReplaceAsync(x => x._id == id, entity);
			return true;
		}
	}
}

