using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Commands.UserCommands.DeleteFeedback;

public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackCommand>
{
	private readonly IUserRepository _userRepository;

	public DeleteFeedbackHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
	{
		await _userRepository.DeleteFeedbackAsync(request.Id);
	}
}