using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Movies.Core;
using Movies.Core.Response;
using Movies.Core.Web;
using Movies.Server.Models;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class AuthenticationController : Controller
	{
		private readonly IDistributedCache _cache;

		public AuthenticationController(IDistributedCache cache)
		{
			_cache = cache;
		}

		[Route("/LoginAsync")]
		[HttpPost]
		public async Task<LoginResponseModel> LoginAsync([FromBody] LoginRequestModel
			request)
		{
			JwtFactory jwtFactory = new JwtFactory();
			if (!ModelState.IsValid)
				throw new Exception(ModelState.Values.ToArray()[0].Errors[0].ErrorMessage);

			if(request.UserName != "RiverTech" || request.Password != "Developer123!!")
				throw new BusinessException(ResponseCodes.NotFound, GetResponseCode.GetDescription(ResponseCodes.NotFound));

			var jwtSecurityToken =
				await jwtFactory.GenerateJwToken(request.UserName, DateTime.Now.AddMinutes(5));
			var refreshToken = jwtFactory.RandomTokenString();

			if(!StringExtensions.IsNullOrEmpty(_cache.GetString("refreshToken")))
			   _cache.Remove("refreshToken");
			_cache.SetString("refreshToken", refreshToken);

			return new LoginResponseModel
			{
				AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				UserName = request.UserName,
				RefreshToken = refreshToken,
				ExpireDateTime = DateTime.Now.AddMinutes(5)
			};
		}

		[Route("/RefreshToken")]
		[HttpPost]
		//[ServiceFilter(typeof(JwtAuthentication))]
		public async Task<RefreshTokenResponseModel> RefreshToken([FromBody] RefreshTokenRequestModel request)
		{
			var cache = _cache.GetString("refreshToken");
			if (_cache.GetString("refreshToken") != request.RefreshToken || request.UserName != "RiverTech")
				throw new BusinessException(ResponseCodes.ExpireToken, "Refresh Token or UserName is wrong");
			JwtFactory jwtFactory = new JwtFactory();
			
			if (!ModelState.IsValid)
				throw new Exception(ModelState.Values.ToArray()[0].Errors[0].ErrorMessage);

			var jwtSecurityToken =
				await jwtFactory.GenerateJwToken(request.UserName, DateTime.Now.AddMinutes(5));

			return new RefreshTokenResponseModel()
			{
				AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
			};
		}
	}
}
