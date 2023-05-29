using MediatR;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
	where TRequest : notnull
	where TResponse : notnull
{
	private readonly ICurrentUserService _currentUserService;

	public AuthorizationBehavior(ICurrentUserService currentUserService)
	{
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		// if (_currentUserService.UserId is null) 
		// 	return await next();
		//
		// if(_currentUserService.Expires is null)
		// 	throw new UnauthorizedAccessException("Token expired");
		//
		// var expires = DateTime.Parse(_currentUserService.Expires);
		//
		// if (DateTime.UtcNow > expires)
		// {
		// 	throw new UnauthorizedAccessException("Token expired");
		// }
		//
		return await next();
	}
}