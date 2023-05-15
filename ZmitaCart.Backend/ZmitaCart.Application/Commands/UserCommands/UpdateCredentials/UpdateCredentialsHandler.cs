using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateCredentials;

public class UpdateCredentialsHandler : IRequestHandler<UpdateCredentialsCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public UpdateCredentialsHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task Handle(UpdateCredentialsCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId == null)
		{
			throw new UnauthorizedAccessException("Log in to use this function");
		}
		
		await _userRepository.UpdateCredentialsAsync(_currentUserService.UserId, request.PhoneNumber, request.Address);
	}
}