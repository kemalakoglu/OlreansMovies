using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core
{
	public interface IJWTConfiguration
	{
		string Key { get; set; }
		string Issuer { get; set; }
		string Audience { get; set; }
		double DurationInMinutes { get; set; }
		int RefreshTokenDurationInDays { get; set; }

	}

	public class JWTConfiguration : IJWTConfiguration
	{
		public JWTConfiguration(IConfiguration config)
		{
			Key = config.GetSection("JWTConfiguration:" + "Key").Get<string>();
			Issuer = config.GetSection("JWTConfiguration:" + "Issuer").Get<string>();
			Audience = config.GetSection("JWTConfiguration:" + "Audience").Get<string>();
			DurationInMinutes = config.GetSection("JWTConfiguration:" + "DurationInMinutes").Get<double>();
			RefreshTokenDurationInDays = config.GetSection("JWTConfiguration:" + "RefreshTokenDurationInDays").Get<int>();
		}


		public string Key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public double DurationInMinutes { get; set; }
		public int RefreshTokenDurationInDays { get; set; }
	}
}
