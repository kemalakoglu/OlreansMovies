using Moq.AutoMock;
using Movies.Contracts;
using Movies.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zeppeling.Framework.Abstactions.Error;

namespace Movies.GrainClients.Test;

public class MovieGrainClientTest
{
	[Fact]
	public async void GetRatedMoviesMethodShouldReturn5Films()
	{
		//Arrange
		var mocker = new AutoMocker();

		IEnumerable<MovieModel> expectedData = new List<MovieModel>()
		{
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel()
		};

		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		movieGrainClient.Setup(x => x.GetRatedMovies()).Returns(Task.FromResult(expectedData)).Verifiable();

		//Act
		var actualData = movieGrainClient.Object.GetRatedMovies().Result;

		//Assert
		Assert.Equal(expectedData.Count(), actualData.Count());
		Assert.NotEmpty(actualData);

		mocker.VerifyAll();
	}

	/// <summary>
	/// </summary>
	/// <param name="Id"></param>
	[Theory]
	[InlineData("1234")]
	public async void GetMethodShouldReturnMovieDataById(string Id)
	{
		//Arrange
		var mocker = new AutoMocker();

		MovieModel expectedData = new MovieModel();

		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		movieGrainClient.Setup(x => x.Get(Id)).Returns(Task.FromResult(expectedData)).Verifiable();

		//Act
		var actualData = movieGrainClient.Object.Get(Id).Result;

		//Assert
		Assert.Equal(expectedData, actualData);
		Assert.NotNull(Id);

		mocker.VerifyAll();
	}

	/// <summary>
	/// </summary>
	/// <param name="Id"></param>
	[Theory]
	[InlineData("1234")]
	public async void GetMethodIdShouldNotBeEmpty(string Id)
	{
		//Assert
		Assert.NotNull(Id);
	}

	[Theory]
	[InlineData("comedy")]
	[InlineData("crime")]
	[InlineData("biography")]
	public async void GetListMethodShouldReturnMovieListByGenreFilter(string genre)
	{
		//Arrange
		var mocker = new AutoMocker();

		IEnumerable<MovieModel> expectedData = new List<MovieModel>()
		{
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel()
		};

		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		movieGrainClient.Setup(x => x.GetList(genre, String.Empty, String.Empty, String.Empty, 0)).Returns(Task.FromResult(expectedData)).Verifiable();

		//Act
		var actualData = movieGrainClient.Object.GetList(genre, String.Empty, String.Empty, String.Empty, 0).Result;

		//Assert
		Assert.Equal(expectedData.Count(), actualData.Count());

		mocker.VerifyAll();
	}

	[Fact]
	public async void GetListMethodShouldReturnFullMovieList()
	{
		//Arrange
		var mocker = new AutoMocker();

		IEnumerable<MovieModel> expectedData = new List<MovieModel>()
		{
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel(),
			new MovieModel()
		};

		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		movieGrainClient.Setup(x => x.GetList(String.Empty, string.Empty, string.Empty, string.Empty, 0)).Returns(Task.FromResult(expectedData)).Verifiable();

		//Act
		var actualData = movieGrainClient.Object.GetList(String.Empty, string.Empty, string.Empty, string.Empty, 0).Result;

		//Assert
		Assert.Equal(expectedData.Count(), actualData.Count());

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldPersistNewEntity()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = movieGrainClient.Object.Set(new MovieModel());

		//Act
		var actual = movieGrainClient.Object.Set(new MovieModel());

		//Assert
		Assert.Equal(expected, actual);

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfIdIsExistInRequestBody()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = new ErrorDTO();
		movieGrainClient.Setup(x => x.Set(new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieGrainClient.Object.Set(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfEntityAlreadyExist()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = new ErrorDTO();
		movieGrainClient.Setup(x => x.Set(new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieGrainClient.Object.Set(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);

	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldPersistExistEntity(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = movieGrainClient.Object.Update(id, new MovieModel());

		//Act
		var actual = movieGrainClient.Object.Update(id, new MovieModel());

		//Assert
		Assert.Equal(expected, actual);

		mocker.VerifyAll();
	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldThrowExceptionIfIdIsMissingInRequestBody(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = new ErrorDTO();
		movieGrainClient.Setup(x => x.Update(id, new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieGrainClient.Object.Update(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldThrowExceptionIfEntityIsntExist(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieGrainClient = mocker.GetMock<IMovieGrainClient>();
		var expected = new ErrorDTO();
		movieGrainClient.Setup(x => x.Update(id, new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieGrainClient.Object.Update(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}
}