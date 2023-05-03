using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.UserCommands.GiveFeedback;

public class GiveFeedbackHandler : IRequestHandler<GiveFeedbackCommand, int>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public GiveFeedbackHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<int> Handle(GiveFeedbackCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You must be logged in to give feedback");
		}
		
		var raterId = int.Parse(_currentUserService.UserId);
		
		return await _userRepository.GiveFeedbackAsync(raterId, request.RecipientId, request.Rating, request.Comment);
	}
}