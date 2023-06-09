using MediatR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Events;

public class JoinedChatHandler : INotificationHandler<JoinedChat>
{
	private readonly IChatHub _chatHub;
	private readonly IConversationRepository _conversationRepository;
	private readonly ICurrentUserService _currentUserService;

	public JoinedChatHandler(IChatHub chatHub, IConversationRepository conversationRepository, ICurrentUserService currentUserService)
	{
		_chatHub = chatHub;
		_conversationRepository = conversationRepository;
		_currentUserService = currentUserService;
	}
	
	public async Task Handle(JoinedChat notification, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId;
		var usersId = await _conversationRepository.GetConversationUserIds(notification.Chat);

		if (userId is null || !usersId.Contains(int.Parse(userId)))
			throw new UnauthorizedAccessException("You are not a member of this chat.");
		
		var messages = await _conversationRepository.GetMessagesAsync(notification.Chat);
		
		foreach (var message in messages)
		{
			await _chatHub.RestoreMessages(message.UserId, message.User, notification.Chat, message.Text);
		}
	}
}