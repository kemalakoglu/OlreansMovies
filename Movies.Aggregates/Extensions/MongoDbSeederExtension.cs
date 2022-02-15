using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Movies.Contracts.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Aggregates.Extensions
{
	public static class MongoDbSeederExtension
	{
		public static void RunMongoDbSeeder(this IServiceCollection services,
			IConfiguration configuration)
		{
			IEnumerable<MovieModel> movies;
			using (StreamReader r = new StreamReader("movies.json"))
			{
				string json = r.ReadToEnd();
				movies = JsonConvert.DeserializeObject<List<MovieModel>>(json);
			}

			var dbClient = new MongoClient(configuration.GetSection("MongoDbSettings:" + "ConnectionString").Get<string>());
			var db = dbClient.GetDatabase(configuration.GetSection("MongoDbSettings:" + "DatabaseName").Get<string>());
			IMongoCollection<MovieModel> _collection = db.GetCollection<MovieModel>("Movies");

			_collection.InsertMany(movies);
		}
	}
}
