using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;

public class UpdateFeedbackHandler : IRequestHandler<UpdateFeedbackCommand, int>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public UpdateFeedbackHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<int> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You must be logged in to update feedback");
		}

		var raterId = int.Parse(_currentUserService.UserId);
		
		return await _userRepository.UpdateFeedbackAsync(request.Id, raterId, request.Rating, request.Comment);
	}
}