using Microsoft.AspNetCore.Http;
using Movies.Core;
using Movies.Core.Response;
using Movies.Core.Web;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Error;

namespace Movies.Server.Extensions;

public class ErrorHandlingMiddleware
{
	private readonly string errorMessageTemplate = "{ErrorMessage} {RequestPath} {Details}";
	private readonly RequestDelegate next;
	private readonly string requestMessageTemplate = "{RequestPath} {RequestBody}";

	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			Log.Write(LogEventLevel.Information, requestMessageTemplate,
				"Service path is:" + context.Request.Path.Value,
				context.Request.Body);
			await next(context);
		}
		catch (HttpRequestException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (AuthenticationException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (BusinessException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (UnauthorizedException ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
		catch (Exception ex)
		{
			Log.Write(LogEventLevel.Error, errorMessageTemplate, ex.Message,
				"Service path is:" + context.Request.Path.Value, ex);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, object exception)
	{
		var code = HttpStatusCode.InternalServerError; // 500 if unexpected
		var message = string.Empty;
		var RC = string.Empty;
		var details = string.Empty;

		if (exception.GetType() == typeof(HttpRequestException))
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Failed;
			message = BusinessException.GetDescription(RC);
			details = ((HttpRequestException)exception).Message;
		}
		else if (exception.GetType() == typeof(AuthenticationException))
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Unauthorized;
			message = BusinessException.GetDescription(RC);
			details = ((AuthenticationException)exception).Message;
		}
		else if (exception.GetType() == typeof(BusinessException))
		{
			var businesException = (BusinessException)exception;
			message = businesException.Message;
			code = HttpStatusCode.OK;
			RC = businesException.RC;
			if (RC == ResponseCodes.ExpireToken || RC == ResponseCodes.InvalidToken || RC == ResponseCodes.Unauthorized)
				code = HttpStatusCode.Unauthorized;
			else if (StringExtensions.IsNullOrEmpty(businesException.RC))
				RC = ResponseCodes.Failed;
			else
				RC = businesException.RC;
			details = ((BusinessException)exception).Details;
		}
		else if (exception.GetType() == typeof(Exception))
		{
			code = HttpStatusCode.BadRequest;
			RC = ResponseCodes.BadRequest;
			message = BusinessException.GetDescription(RC);
			details = ((Exception)exception).Message;
		}
		else
		{
			code = HttpStatusCode.OK;
			RC = ResponseCodes.Failed;
			message = BusinessException.GetDescription(RC);
		}

		var response = new ErrorDTO
		{
			message = message, rc = RC, details = details, trackId = Guid.NewGuid().ToString()
		};
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)code;
		return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
	}
}