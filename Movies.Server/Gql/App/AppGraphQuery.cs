using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.Server.Gql.App
{
	public class AppGraphQuery : ObjectGraphType
	{
		public AppGraphQuery(IMovieGrainClient movieGrainClient)
		{
			Name = "AppQueries";

			Field<MovieGraphType>(
				"get",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "id"
				}),
				resolve: ctx => movieGrainClient.Get(ctx.Arguments["id"].ToString())
			);

			Field<ListGraphType<MovieGraphType>>("getList",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "genre"
				}),
				resolve: ctx => movieGrainClient.GetList(ctx.Arguments["genre"].ToString())
			);

			Field<ListGraphType<MovieGraphType>>("getRatedFilms",
				arguments: new QueryArguments(new QueryArgument<StringGraphType>
				{
					Name = "genre"
				}),
				resolve: ctx => movieGrainClient.GetRatedFilms()
			);
		}
	}
}
