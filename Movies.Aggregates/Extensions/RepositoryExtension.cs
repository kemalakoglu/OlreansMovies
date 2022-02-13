using Microsoft.Extensions.DependencyInjection;
using Movies.Aggregates.Film;
using Movies.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Aggregates.Extensions
{
	public static class RepositoryExtension
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IMovieRepository, MovieRepository>();
		}
	}
}
