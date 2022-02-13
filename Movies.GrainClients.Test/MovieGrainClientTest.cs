using System;
using Xunit;

namespace Movies.GrainClients.Test
{
    public class MovieGrainClientTest
    {
		[Fact]
		public async void GetRatedMoviesMethodShouldReturnFilmList()
		{
			//todo
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
