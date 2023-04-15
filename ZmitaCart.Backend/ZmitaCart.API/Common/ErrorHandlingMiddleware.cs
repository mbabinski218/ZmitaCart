using System.Text.Json;
using FluentValidation;
using ZmitaCart.Infrastructure.Exceptions;

namespace ZmitaCart.API.Common;

public class ErrorHandlingMiddleware : IMiddleware
{
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

	public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (NotImplementedException)
		{
			context.Response.StatusCode = StatusCodes.Status501NotImplemented;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync("Not implemented yet.");
		}
		catch (ValidationException ex)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonSerializer
				.Serialize(ex.Errors.Select(vf => vf.ErrorMessage)));
		}
		catch (InvalidDataException ex)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync(ex.Message);
		}
		catch (InvalidLoginDataException ex)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonSerializer.Serialize(ex.Errors));
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{msg}", ex.Message);
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync("An unexpected error occurred.");
		}
	}
}