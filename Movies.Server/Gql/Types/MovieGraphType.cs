using GraphQL.Types;
using Movies.Contracts.Entity;

namespace Movies.Server.Gql.Types;

public class MovieGraphType : ObjectGraphType<MovieModel>
{
	public MovieGraphType()
	{
		Field(x => x._id, true).Description("Unique key.");
		Field(x => x.name, true).Description("Name.");
		Field(x => x.key, true).Description("Key.");
		Field(x => x.description, true).Description("Description.");
		Field(x => x.genres, nullable: true).Description("Genres.");
		Field(x => x.rate, true).Description("Rate.");
		Field(x => x.length, true).Description("Length.");
		Field(x => x.img, true).Description("Img.");
	}
}