using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.Contracts.Entity;
using Movies.Server.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers;

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
	[ServiceFilter(typeof(JwtAuthentication))]
	public async Task<IEnumerable<MovieModel>> GetRatedMovies() => await _client.GetRatedMovies().ConfigureAwait(false);

	// GET api/sampledata/1234
	[HttpGet]
	[Route("movie/{id}")]
	public async Task<MovieModel> Get([FromRoute] string id)
	{
		var result = await _client.Get(id).ConfigureAwait(false);
		return result;
	}

	[HttpGet]
	[ServiceFilter(typeof(JwtAuthentication))]
	[Route("filter/")]
	public async Task<IEnumerable<MovieModel>> GetList(string genre, string name, string key, string description, double rate) => await _client.GetList(genre, name, key, description, rate).ConfigureAwait(false);

		

	[HttpPost]
	[ServiceFilter(typeof(JwtAuthentication))]
	public async Task<MovieModel> Set([FromBody] MovieModel entity) => await _client.Set(entity).ConfigureAwait(false);

	[HttpPut("{id}")]
	[ServiceFilter(typeof(JwtAuthentication))]
	public async Task<MovieModel> Update([FromRoute] string id, [FromBody] MovieModel entity) =>
		await _client.Update(id, entity).ConfigureAwait(false);
}