using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Server.Gql.Types;
using System.Threading.Tasks;

namespace Movies.Server.Gql.App;

public class AppGraphMutation : ObjectGraphType
{
	public AppGraphMutation(IMovieGrainClient movieGrainClient)
	{
		Name = "AppMutations";

		Field<MovieGraphType>(
			"set",
			arguments: new QueryArguments(
				new QueryArgument<NonNullGraphType<MovieGraphInputType>> { Name = "movieModel" }),
			resolve: ctx => movieGrainClient.Set(ctx.GetArgument<MovieModel>("movieModel"))
			);

		Field<MovieGraphType>("update",
			arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" },
				new QueryArgument<NonNullGraphType<MovieGraphInputType>> { Name = "movieModel" }),
			resolve: ctx => movieGrainClient.Update(ctx.GetArgument<string>("id"), ctx.GetArgument<MovieModel>("movieModel"))
            );

	}
}