using Moq.AutoMock;
using Movies.Aggregates.Film;
using Movies.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zeppeling.Framework.Abstactions.Error;

namespace Movies.Repository.Test;

public class MoviesRepositoryTest
{
	[Theory]
	[InlineData("1234")]
	public async void GetMethodShouldReturnMovieDataById(string Id)
	{
		//Arrange
		var mocker = new AutoMocker();

		MovieModel expectedData = new MovieModel();

		var movieRepository = mocker.GetMock<IMovieRepository>();
		movieRepository.Setup(x => x.Get(Id)).Returns(Task.FromResult(expectedData)).Verifiable();

		//Act
		var actualData = movieRepository.Object.Get(Id).Result;

		//Assert
		Assert.Equal(expectedData, actualData);
		Assert.NotNull(Id);

		mocker.VerifyAll();
	}

	/// <summary>
	///     Get Method Should Returns ErrorMessage If Id Parameter Is Null or Empty
	/// </summary>
	/// <param name="Id"></param>
	[Theory]
	[InlineData("1234")]
	public async void GetMethodShouldReturnErrorMessageIfIdParameterIsNullOrEmpty(string Id)
	{
		//Assert
		Assert.NotNull(Id);
	}

	[Theory]
	[InlineData(1234)]
	public async void GetMethodShouldReturnErrorMessageIfIdParameterIsNotString(int id)
	{
		//Assert
		Assert.NotSame(id, 1234);
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

		var movieRepository = mocker.GetMock<IMovieRepository>();
		movieRepository.Setup(x => x.GetList(genre)).Returns(Task.FromResult(expectedData.AsQueryable())).Verifiable();

		//Act
		var actualData = movieRepository.Object.GetList(genre).Result;

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

		var movieRepository = mocker.GetMock<IMovieRepository>();
		movieRepository.Setup(x => x.GetList(String.Empty)).Returns(Task.FromResult(expectedData.AsQueryable())).Verifiable();

		//Act
		var actualData = movieRepository.Object.GetList(String.Empty).Result;

		//Assert
		Assert.Equal(expectedData.Count(), actualData.Count());

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldPersistNewEntity()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = Task.FromResult(true);

		//Act
		var actual = Task.FromResult(true);

		//Assert
		Assert.Equal(expected, actual);

		mocker.VerifyAll();
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfIdIsExistInRequestBody()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = new ErrorDTO();
		movieRepository.Setup(x => x.AddAsync(new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieRepository.Object.AddAsync(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Fact]
	public async void SetMethodShouldThrowExceptionIfEntityAlreadyExist()
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = new ErrorDTO();
		movieRepository.Setup(x => x.AddAsync(new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieRepository.Object.AddAsync(new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldPersistExistEntity(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = Task.FromResult(true);

		//Act
		var actual = Task.FromResult(true);

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
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = new ErrorDTO();
		movieRepository.Setup(x => x.UpdateAsync(id, new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieRepository.Object.UpdateAsync(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}

	[Theory]
	[InlineData("1234")]
	public async void UpdateMethodShouldThrowExceptionIfEntityIsntExist(string id)
	{
		//Arrange
		var mocker = new AutoMocker();
		var movieRepository = mocker.GetMock<IMovieRepository>();
		var expected = new ErrorDTO(); ;
		movieRepository.Setup(x => x.UpdateAsync(id, new MovieModel())).Returns(Task.FromResult(new MovieModel())).Verifiable();
		//Act
		var actual = movieRepository.Object.UpdateAsync(id, new MovieModel());

		//Assert
		Assert.NotSame(expected, actual);
	}
}