using Movies.Contracts;
using Movies.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Aggregates.Film
{
	public interface IMovieRepository
	{
		Task<IQueryable<MovieModel>> GetList(string genre);
		Task<bool> AddAsync(MovieModel entity);
		Task<bool> UpdateAsync(string id, MovieModel entity);
		Task<MovieModel> Get(string id);
		Task<IQueryable<MovieModel>> GetRatedMovies();
	}
}
