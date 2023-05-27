using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.UserQueries.LogoutUser;

public class LogoutUserHandler : IRequestHandler<LogoutUserQuery, Result>
{
	private readonly IUserRepository _userRepository;

	public LogoutUserHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
	{
		return await _userRepository.LogoutAsync();
	}
}