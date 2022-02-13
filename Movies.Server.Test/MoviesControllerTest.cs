using Moq.AutoMock;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Server.Test
{
	public class MoviesControllerTest
	{
		[Fact]
		public async void GetRatedMoviesMethodShouldReturnFilmList()
		{
			////Arrange
			//var mocker = new AutoMocker();
			//var movieModel = mocker.CreateInstance<IEnumerable<Contracts.Entity.Movies>>();
			//var movieController = mocker.GetMock<MoviesController>();
			//movieController.Setup(x => x.GetRatedMovies()).Returns(Task.FromResult(movieModel)).Verifiable();

			////Act
			//var response = await movieController.Object.GetRatedMovies();

			////Assert
			//Assert.Equal(movieModel.Count(), response.Count());

			//mocker.VerifyAll();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		[Theory]
		[InlineData("1234")]
		public async void GetMethodShouldReturnMovieDataById(string Id)
		{
			//todo
		}

		/// <summary>
		/// Get Method Should Returns ErrorMessage If Id Parameter Is Null or Empty
		/// </summary>
		/// <param name="Id"></param>
		[Fact]
		public async void GetMethodShouldReturnErrorMessageIfIdParameterIsNullOrEmpty()
		{
			//todo
		}

		[Fact]
		public async void GetMethodShouldReturnErrorMessageIfIdParameterIsNotString()
		{
			//todo
		}

		[Theory]
		[InlineData("comedy")]
		public async void GetListMethodShouldReturnMovieListByGenreFilter(string genre)
		{
			// todo
		}

		[Fact]
		public async void GetListMethodShouldReturnFullMovieList()
		{
			// todo
		}

		[Fact]
		public async void SetMethodShouldPersistNewEntity()
		{
			// todo
		}

		[Fact]
		public async void SetMethodShouldThrowExceptionIfIdIsExistInRequestBody()
		{
			// todo
		}

		[Fact]
		public async void SetMethodShouldThrowExceptionIfEntityAlreadyExist()
		{
			// todo
		}

		[Fact]
		public async void UpdateMethodShouldPersistExistEntity()
		{
			// todo
		}

		[Fact]
		public async void UpdateMethodShouldThrowExceptionIfIdIsMissingInRequestBody()
		{
			// todo
		}

		[Fact]
		public async void UpdateMethodShouldThrowExceptionIfEntityIsntExist()
		{
			// todo
		}
	}
}
