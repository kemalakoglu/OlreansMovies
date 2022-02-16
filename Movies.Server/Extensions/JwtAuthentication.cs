using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Movies.Core;
using Movies.Core.Response;
using Movies.Core.Web;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Movies.Server.Extensions
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class JwtAuthentication : ActionFilterAttribute
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <exception cref="BusinessException">
		/// Your Token is: " + key
		/// or
		/// </exception>
		/// <inheritdoc />
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			try
			{
				var controller = (Controller)context.Controller;
					var key = controller.Request.Headers.ToArray().FirstOrDefault(x => x.Key == "Authorization").Value
						.ToString();

					if (!ValidateToken(key) || StringExtensions.IsNullOrEmpty(key))
						throw new BusinessException(ResponseCodes.InvalidToken, "Your Token is: " + key);
			}
			catch (Exception e)
			{
				throw new BusinessException(ResponseCodes.Unauthorized, GetResponseCode.GetDescription(ResponseCodes.InvalidToken));
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <inheritdoc />
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			//Log.Write(LogEventLevel.Information, "Jwt token is succeed.");
		}

		/// <summary>
		/// Validates the token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public bool ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			if (jwtToken == null)
				throw new BusinessException(ResponseCodes.InvalidToken, GetResponseCode.GetDescription(ResponseCodes.InvalidToken));

			var expireDate = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp");
			if (expireDate == null)
				throw new BusinessException(ResponseCodes.InvalidToken, GetResponseCode.GetDescription(ResponseCodes.InvalidToken));
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expireDate.Value)).LocalDateTime;

			if (dateTimeOffset < DateTimeOffset.Now)
				throw new BusinessException(ResponseCodes.InvalidToken, GetResponseCode.GetDescription(ResponseCodes.InvalidToken));

			if (jwtToken.Claims.FirstOrDefault(x => x.Type == "userName").Value != "RiverTech")
				throw new BusinessException(ResponseCodes.InvalidToken, GetResponseCode.GetDescription(ResponseCodes.InvalidToken));

			return true;
		}
	}
}
