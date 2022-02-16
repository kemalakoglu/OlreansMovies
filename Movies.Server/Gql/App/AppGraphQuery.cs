using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;
using System;

namespace Movies.Server.Gql.App;

public class AppGraphQuery : ObjectGraphType
{
	public AppGraphQuery(IMovieGrainClient movieGrainClient)
	{
		Name = "AppQueries";

		Field<MovieGraphType>(
			"get",
			arguments: new QueryArguments(new QueryArgument<StringGraphType> {Name = "id"}),
			resolve: ctx => movieGrainClient.Get(ctx.Arguments["id"].ToString())
		);

		Field<ListGraphType<MovieGraphType>>("getList",
			arguments: new QueryArguments(new QueryArgument<StringGraphType> {Name = "genre"},
				new QueryArgument<StringGraphType> { Name = "name" },
				new QueryArgument<StringGraphType> { Name = "key" },
				new QueryArgument<StringGraphType> { Name = "description" },
				new QueryArgument<FloatGraphType> { Name = "rate" }),
			resolve: ctx => movieGrainClient.GetList(ctx.Arguments["genre"].ToString(), ctx.Arguments["name"].ToString(), 
				ctx.Arguments["key"].ToString(), ctx.Arguments["description"].ToString(), 
				Convert.ToDouble(!string.IsNullOrEmpty(ctx.Arguments["rate"].ToString()) ? string.IsNullOrEmpty(ctx.Arguments["rate"].ToString()) : 0))
		);

		Field<ListGraphType<MovieGraphType>>("getRatedFilms",
			resolve: ctx => movieGrainClient.GetRatedMovies()
		);
	}
}