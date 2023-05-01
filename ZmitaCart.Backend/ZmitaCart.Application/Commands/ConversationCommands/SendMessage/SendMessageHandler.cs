using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.ConversationCommands.SendMessage;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, int>
{
	private readonly IConversationRepository _conversationRepository;
	private readonly ICurrentUserService _currentUserService;

	public SendMessageHandler(IConversationRepository conversationRepository, ICurrentUserService currentUserService)
	{
		_conversationRepository = conversationRepository;
		_currentUserService = currentUserService;
	}

	public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId == null)
		{
			throw new UnauthorizedAccessException("User is not logged in");
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		return await _conversationRepository.SendMessageAsync(userId, request.ConversationId, request.Text);
	}
}