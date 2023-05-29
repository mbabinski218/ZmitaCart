﻿using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ZmitaCart.API.Common;

public static class ResponseHandler
{
	public static ActionResult HandleErrors(IEnumerable<IError> errors)
	{
		var errorsMessages = errors.Select(e => e.Message);
		return new BadRequestObjectResult(errorsMessages);
	}
}