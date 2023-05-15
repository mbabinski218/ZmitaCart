using FluentResults;
using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Common.Errors;

public class NotFoundError : IError
{
	public NotFoundError(string message)
	{
		Message = message;
	}
	
	public string Message { get; }
	public Dictionary<string, object>? Metadata { get; set; }
	public List<IError>? Reasons { get; set; }
	public int StatusCode => StatusCodes.Status404NotFound;
}