using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Moq.AutoMock;
using Movies.Server.Gql.Types;
using Xunit;

namespace Movies.Server.Test;

public class MoviesGraphTest
{
	private readonly IGraphQLClient _client;

	public MoviesGraphTest()
	{
		_client = new GraphQLHttpClient("http://localhost:6600/graphql", new NewtonsoftJsonSerializer());
	}

	/// <summary>
	/// </summary>
	/// <param name="Id"></param>
	[Theory]
	[InlineData("1234")]
	public async void GetFieldShouldReturnMovieDataById(string Id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieModel = mocker.CreateInstance<MovieGraphType>();

		//Act
		var query = new GraphQLRequest
		{
			Query = @"query get($id: String!) {
                                movie(id: $id) {
                                          Id,
                                          Name,
                                          Key,
                                          Description,
                                          Genres,
                                          Rate,
                                          Length,
                                          Img
                                          }
                                }"
		};
		var response = await _client.SendQueryAsync<MovieGraphType>(query);

		//Assert
		Assert.Equal(movieModel, response.Data);

		mocker.VerifyAll();
	}

	/// <summary>
	///     Get Method Should Returns ErrorMessage If Id Parameter Is Null or Empty
	/// </summary>
	/// <param name="Id"></param>
	[Fact]
	public async void GetFieldShouldReturnErrorMessageIfIdParameterIsNullOrEmpty()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieModel = mocker.CreateInstance<MovieGraphType>();

		//Act
		var query = new GraphQLRequest
		{
			Query = @"query get($id: String!) {
                                movie(id: $id) {
                                          Id,
                                          Name,
                                          Key,
                                          Description,
                                          Genres,
                                          Rate,
                                          Length,
                                          Img
                                          }
                                }"
		};
		var response = await _client.SendQueryAsync<MovieGraphType>(query);

		//Assert
		Assert.Equal("Id should be passed", "Id should be passed");

		mocker.VerifyAll();
	}

	[Fact]
	public async void GetFieldShouldReturnErrorMessageIfIdParameterIsNotString()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieModel = mocker.CreateInstance<MovieGraphType>();

		//Act
		var query = new GraphQLRequest
		{
			Query = @"query get($id: String!) {
                                movie(id: $id) {
                                          Id,
                                          Name,
                                          Key,
                                          Description,
                                          Genres,
                                          Rate,
                                          Length,
                                          Img
                                          }
                                }"
		};
		var response = await _client.SendQueryAsync<MovieGraphType>(query);

		//Assert
		Assert.Equal("Id should be string", "Id should be string");

		mocker.VerifyAll();
	}

	/// <summary>
	/// </summary>
	[Fact]
	public async void GetRatedFilmsFieldShouldReturnMovieList()
	{
		// todo
	}

	[Theory]
	[InlineData("comedy")]
	public async void GetListFieldShouldReturnMovieListByGenreFilter(string genre)
	{
		// todo
	}

	[Fact]
	public async void GetListFieldShouldReturnFullMovieList()
	{
		// todo
	}

	[Fact]
	public async void SetMutationShouldPersistNewEntity()
	{
		// todo
	}

	[Fact]
	public async void SetMutationShouldThrowExceptionIfIdIsExistInRequestBody()
	{
		// todo
	}

	[Fact]
	public async void SetMutationShouldThrowExceptionIfEntityAlreadyExist()
	{
		// todo
	}

	[Fact]
	public async void UpdateMutationShouldPersistExistEntity()
	{
		// todo
	}

	[Fact]
	public async void UpdateMutationShouldThrowExceptionIfIdIsMissingInRequestBody()
	{
		// todo
	}

	[Fact]
	public async void UpdateMutationShouldThrowExceptionIfEntityIsntExist()
	{
		// todo
	}
}