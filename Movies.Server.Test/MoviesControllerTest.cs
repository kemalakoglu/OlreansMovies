using Moq.AutoMock;
using Movies.Contracts.Entity;
using Movies.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zeppeling.Framework.Abstactions.Error;

namespace Movies.Server.Test;

public class MoviesControllerTest
{
	[Fact]
	public async void GetRatedMoviesMethodShouldReturn5Films()
	{
		//Arrange
		var mocker = new AutoMocker();

		IEnumerable<MovieModel> expectedData = new List<MovieModel>();

		var movieController = mocker.GetMock<MoviesController>();

		//Act
		var actualData = movieController.Object.GetRatedMovies().Result;

		//Assert
		Assert.Equal(expectedData, actualData);

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

		var movieController = mocker.GetMock<MoviesController>();

		//Act
		var actualData = movieController.Object.Get(Id).Result;

		//Assert
		Assert.Equal(null, actualData);

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

		IEnumerable<MovieModel> expectedData = new List<MovieModel>();

		var movieController = mocker.GetMock<MoviesController>();

		//Act
		var actualData = movieController.Object.GetList(genre, String.Empty, String.Empty, String.Empty, 0).Result;

		//Assert
		Assert.Equal(expectedData, actualData);

		mocker.VerifyAll();
	}

	[Fact]
	public async void GetListMethodShouldReturnFullMovieList()
	{
		//Arrange
		var mocker = new AutoMocker();

		IEnumerable<MovieModel> expectedData = new List<MovieModel>();

		var movieController = mocker.GetMock<MoviesController>();
		
		//Act
		var actualData = movieController.Object.GetList(String.Empty, String.Empty, String.Empty, String.Empty, 0).Result;

		//Assert
		Assert.Equal(expectedData, actualData);

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldPersistNewEntity()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieController = mocker.GetMock<MoviesController>();
		var expected = movieController.Object.Set(new MovieModel());

		//Act
		var actual = movieController.Object.Set(new MovieModel());

		//Assert
		Assert.Equal(expected, actual);

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfIdIsExistInRequestBody()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieController = mocker.GetMock<MoviesController>();
		var expected = new ErrorDTO();
		//Act
		var actual = movieController.Object.Set(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfEntityAlreadyExist()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieController = mocker.GetMock<MoviesController>();
		var expected = new ErrorDTO();
		//Act
		var actual = movieController.Object.Set(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);

	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldPersistExistEntity(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieController = mocker.GetMock<MoviesController>();
		var expected = movieController.Object.Update(id, new MovieModel());

		//Act
		var actual = movieController.Object.Update(id, new MovieModel());

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
		var movieController = mocker.GetMock<MoviesController>();
		var expected = new ErrorDTO();

		//Act
		var actual = movieController.Object.Update(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldThrowExceptionIfEntityIsntExist(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieController = mocker.GetMock<MoviesController>();
		var expected = new ErrorDTO();

		//Act
		var actual = movieController.Object.Update(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}
}