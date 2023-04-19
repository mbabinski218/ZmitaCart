using System.Security.Authentication;
using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.UserCommands.SetPhoneNumber;

public class SetPhoneNumberHandler : IRequestHandler<SetPhoneNumberCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public SetPhoneNumberHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task Handle(SetPhoneNumberCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId == null)
		{
			throw new UnauthorizedAccessException("Log in to use this function");
		}
		
		await _userRepository.SetPhoneNumberAsync(_currentUserService.UserId, request.PhoneNumber);
	}
}