using System.Collections.Generic;

namespace Movies.Contracts
{
	public class MovieModel
	{
		public string Id { get; protected set; }
		public string Name { get; protected set; }
		public string Key { get; protected set; }
		public string Description { get; protected set; }
		public IEnumerable<Genre> Genres { get; protected set; }
		public double Rate { get; protected set; }
		public string Length { get; protected set; }
		public string Img { get; protected set; }

		public MovieModel() { }

		public MovieModel(string Id, string Name, string Key, string Description, double Rate, string Length, string Img, IEnumerable<Genre> Genres)
		{
			this.Id = Id;
			this.Name = Name;
			this.Key = Key;
			this.Description = Description;
			this.Rate = Rate;
			this.Length = Length;
			this.Img = Img;
			this.Genres = Genres;
		}
	}
}
