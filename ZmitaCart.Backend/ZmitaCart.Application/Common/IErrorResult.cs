using FluentResults;

namespace ZmitaCart.Application.Common;

public interface IErrorResult : IError
{
	public int StatusCode { get; }
}