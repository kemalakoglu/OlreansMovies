using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Web
{
	public class JwtFactory
	{
		public async Task<JwtSecurityToken> GenerateJwToken(string userName, DateTime expireDate)
		{
			var claims = new[]
			{
				new Claim("userName", userName),

			};

			JWTConfiguration jwtConfiguration = PrepareConfiguration();

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				jwtConfiguration.Issuer,
				jwtConfiguration.Audience,
				claims,
				expires: expireDate,
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		public static IConfiguration GetProductionConfiguration()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();
		}

		public static IConfiguration GetDevelopmentConfiguration()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.dev.json", optional: true)
				.Build();
		}

		private JWTConfiguration PrepareConfiguration()
		{
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
				return new JWTConfiguration(GetDevelopmentConfiguration());
			else
				return new JWTConfiguration(GetProductionConfiguration());
		}

		public string RandomTokenString()
		{
			using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			var randomBytes = new byte[40];
			rngCryptoServiceProvider.GetBytes(randomBytes);

			return Convert.ToBase64String(randomBytes);
		}
	}
}
