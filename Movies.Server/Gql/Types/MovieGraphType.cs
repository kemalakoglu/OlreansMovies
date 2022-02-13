using GraphQL.Types;
using Movies.Aggregates.Film;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<MovieModel>
	{
		public MovieGraphType()
		{
			Name = "Movie";
			Description = "Movie Data Type";

			Field(x => x._id, nullable: true).Description("Unique key.");
			Field(x => x.name, nullable: true).Description("Name.");
			Field(x => x.key, nullable: true).Description("Key.");
			Field(x => x.description, nullable: true).Description("Description.");
			// Field(x => x.genres, nullable: true).Description("Genres.");
			Field(x => x.rate, nullable: true).Description("Rate.");
			Field(x => x.length, nullable: true).Description("Length.");
			Field(x => x.img, nullable: true).Description("Img.");
		}
	}
}