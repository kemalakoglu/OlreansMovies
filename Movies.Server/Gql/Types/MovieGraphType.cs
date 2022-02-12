using GraphQL.Types;
using Movies.Contracts;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphType : ObjectGraphType<MovieModel>
	{
		public MovieGraphType()
		{
			Name = "Movie";
			Description = "Movie Data Type";

			Field(x => x.Id, nullable: true).Description("Unique key.");
			Field(x => x.Name, nullable: true).Description("Name.");
			Field(x => x.Key, nullable: true).Description("Key.");
			Field(x => x.Description, nullable: true).Description("Description.");
			Field(x => x.Genres, nullable: true).Description("Genres.");
			Field(x => x.Rate, nullable: true).Description("Rate.");
			Field(x => x.Length, nullable: true).Description("Length.");
			Field(x => x.Img, nullable: true).Description("Img.");
		}
	}
}