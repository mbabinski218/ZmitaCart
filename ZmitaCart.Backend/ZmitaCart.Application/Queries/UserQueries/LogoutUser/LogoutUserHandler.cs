using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.UserQueries.LogoutUser;

public class LogoutUserHandler : IRequestHandler<LogoutUserQuery>
{
	private readonly IUserRepository _userRepository;

	public LogoutUserHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(LogoutUserQuery request, CancellationToken cancellationToken)
	{
		await _userRepository.LogoutAsync();
	}
}