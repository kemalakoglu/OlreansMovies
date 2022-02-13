using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Movies.Contracts.Entity
{
	[Serializable()]
	public class MovieModel
	{
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonId]
		[BsonIgnoreIfDefault]
		[BsonElement(Order = 0)]
		public string _id { get; set; }
		public string name { get; set; }
		public string key { get; set; }
		public string description { get; set; }
		public IEnumerable<string> genres { get; set; }
		public double rate { get; set; }
		public string length { get; set; }
		public string img { get; set; }

		public MovieModel() { }

		public MovieModel(string Name, string Key, string Description, double Rate, string Length, string Img, IEnumerable<string> Genres)
		{
			this.name = Name;
			this.key = Key;
			this.description = Description;
			this.rate = Rate;
			this.length = Length;
			this.img = Img;
			this.genres = Genres;
		}
	}
}
