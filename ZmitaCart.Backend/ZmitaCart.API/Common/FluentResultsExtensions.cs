using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZmitaCart.API.Common;

public class Success<T>
{
	public T Value { get; set; }
	public IReadOnlyCollection<ISuccess> Successes { get; set; }
	public IReadOnlyCollection<IReason> Reasons { get; set; }

	public Success(IResult<T> result)
	{
		Value = result.Value;
		Successes = result.Successes;
		Reasons = result.Reasons;
	}
}

public class Success
{
	public IReadOnlyCollection<ISuccess> Successes { get; set; }
	public IReadOnlyCollection<IReason> Reasons { get; set; }

	public Success(IResultBase result)
	{
		Successes = result.Successes;
		Reasons = result.Reasons;
	}
}

public class Error
{
	public IReadOnlyCollection<IError> Errors { get; set; }
	public IReadOnlyCollection<IReason> Reasons { get; set; }

	public Error(IResultBase result)
	{
		Errors = result.Errors;
		Reasons = result.Reasons;
	}
}

public static class FluentResultsExtensions
{
	public static async Task<ActionResult> Then<T>(this Task<Result<T>> result, Func<Success<T>, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		return response.IsSuccess ? success(new Success<T>(response)) : err(new Error(response));
	}
	
	public static async Task<ActionResult> Then(this Task<Result> result, Func<Success, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		return response.IsSuccess ? success(new Success(response)) : err(new Error(response));
	}
	
	public static List<string> ToList(this Error error)
	{
		return error.Errors.Select(e => e.Message).ToList();
	}
}