using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MoviesController : Controller
	{
		private readonly IMovieGrainClient _client;

		public MoviesController(
			IMovieGrainClient client
		)
		{
			_client = client;
		}

		[HttpGet]
		public async Task<IEnumerable<MovieModel>> GetRatedMovies()
		{
			return new List<MovieModel>();
		}

		// GET api/sampledata/1234
		[HttpGet("{id}")]
		public async Task<MovieModel> Get(string id)
		{
			var result = await _client.Get(id).ConfigureAwait(false);
			return result;
		}

		[HttpGet]
		public async Task<IEnumerable<MovieModel>> GetAll(string queries)
		{
			return new List<MovieModel>();
		}

		// POST api/sampledata/1234
		[HttpPost("{id}")]
		public async Task Set([FromRoute] string id, [FromForm] string name)
			=> await _client.Set(id, name).ConfigureAwait(false);
	}
}