using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ZmitaCart.API.Common;

public class Success<T>
{
	public T Value { get; set; }
	public IReadOnlyCollection<ISuccess> Reasons { get; set; }

	public Success(IResult<T> result)
	{
		Value = result.Value;
		Reasons = result.Successes;
	}
}

public class Success
{
	public IReadOnlyCollection<ISuccess> Reasons { get; set; }

	public Success(IResultBase result)
	{
		Reasons = result.Successes;
	}
}

public class Error
{
	public IReadOnlyCollection<IError> Reasons { get; set; }

	public Error(IResultBase result)
	{
		Reasons = result.Errors;
	}
}

public static class FluentResultsExtensions
{
	public static async Task<ActionResult> Then<T>(this Task<Result<T>> result, Func<Success<T>, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		var status = response.IsSuccess ? success(new Success<T>(response)) : err(new Error(response));
		return status;
	}
	
	public static async Task<ActionResult> Then(this Task<Result> result, Func<Success, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		var status =  response.IsSuccess ? success(new Success(response)) : err(new Error(response));
		return status;
	}
	
	public static List<string> ToList(this Error error)
	{
		var errors = new List<string>();
		foreach (var reason in error.Reasons)
		{
			if (reason.Reasons != null && reason.Reasons.Any())
			{
				errors.AddRange(reason.Reasons.Select(e => e.Message).ToList());
			}
			else
			{
				errors.Add(reason.Message);
			}
		}

		return errors.Take(1).ToList();
	}
}