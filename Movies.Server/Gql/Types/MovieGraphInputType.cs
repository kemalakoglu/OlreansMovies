using GraphQL.Types;

namespace Movies.Server.Gql.Types
{
	public class MovieGraphInputType : InputObjectGraphType
	{
		public MovieGraphInputType()
		{
			Name = "movieModelInput";
			Field<NonNullGraphType<StringGraphType>>("key");
			Field<NonNullGraphType<StringGraphType>>("name");
			Field<NonNullGraphType<StringGraphType>>("description");
			Field<NonNullGraphType<StringGraphType>>("genres");
			Field<NonNullGraphType<StringGraphType>>("rate");
			Field<NonNullGraphType<StringGraphType>>("length");
			Field<NonNullGraphType<StringGraphType>>("img");
		}
	}
}
