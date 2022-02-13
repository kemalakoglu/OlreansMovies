using Microsoft.AspNetCore.Http;
using Movies.Core.Response;
using Movies.Core.Web;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Error;

namespace Movies.Server.Extensions
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly string messageTemplate = "{RequestMethod} {RequestPath} {RequestBody} {HttpStatus} {ResponseBody} {ElapsedTime}";

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var responseBody = new MemoryStream();
			var sw = Stopwatch.StartNew();
			string bodyStr = string.Empty;
			Stream originalBodyStream = null;
			try
			{
				bodyStr = await FormatRequest(context.Request);

				context.Request.Body.Position = 0;
				originalBodyStream = context.Response.Body;

				context.Response.Body = responseBody;
				await next(context);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}
			catch (HttpRequestException ex)
			{
				//Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
				await HandleExceptionAsync(context, ex);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}
			catch (AuthenticationException ex)
			{
				//Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
				await HandleExceptionAsync(context, ex);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}
			catch (BusinessException ex)
			{
				//Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
				await HandleExceptionAsync(context, ex);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}
			catch (UnauthorizedException ex)
			{
				//Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
				await HandleExceptionAsync(context, ex);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}
			catch (Exception ex)
			{
				//Log.Write(LogEventLevel.Error, ex.Message, ex.Source, ex.TargetSite, ex);
				await HandleExceptionAsync(context, ex);
				sw.Stop();
				Log.Write(LogEventLevel.Information, messageTemplate, context.Request.Method, context.Request.Path, bodyStr, context.Response.StatusCode, await FormatResponse(context.Response), sw.Elapsed.TotalMilliseconds);
				await responseBody.CopyToAsync(originalBodyStream);
				responseBody.Dispose();
			}

		}

		private static Task HandleExceptionAsync(HttpContext context, object exception)
		{
			var code = HttpStatusCode.InternalServerError; // 500 if unexpected
			string message = string.Empty;
			string RC = string.Empty;
			string details = string.Empty;

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
				else if (string.IsNullOrEmpty(businesException.RC))
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
				message = message,
				rc = RC,
				details = details,
				trackId = Guid.NewGuid().ToString()
			};
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
		}

		private async Task<string> FormatRequest(HttpRequest request)
		{
			request.EnableBuffering();

			var body = request.Body;

			var buffer = new byte[Convert.ToInt32(request.ContentLength)];
			await request.Body.ReadAsync(buffer, 0, buffer.Length);
			var bodyAsText = Encoding.UTF8.GetString(buffer);
			body.Seek(0, SeekOrigin.Begin);
			request.Body = body;

			return bodyAsText;
		}

		private async Task<string> FormatResponse(HttpResponse response)
		{
			response.Body.Seek(0, SeekOrigin.Begin);
			var text = await new StreamReader(response.Body).ReadToEndAsync();
			response.Body.Seek(0, SeekOrigin.Begin);

			return text;
		}
	}
}
