using MediatR;
using Microsoft.IdentityModel.Tokens;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Events;

public class JoinedChatHandler : INotificationHandler<JoinedChat>
{
	private readonly IChatHub _chatHub;
	private readonly IConversationRepository _conversationRepository;

	public JoinedChatHandler(IChatHub chatHub, IConversationRepository conversationRepository, ICurrentUserService currentUserService)
	{
		_chatHub = chatHub;
		_conversationRepository = conversationRepository;
	}
	
	public async Task Handle(JoinedChat notification, CancellationToken cancellationToken)
	{
		var usersId = (await _conversationRepository.GetConversationUserIds(notification.Chat)).ToList();
		
		if (!usersId.Contains(int.Parse(notification.UserId)))
			throw new UnauthorizedAccessException("You are not a member of this chat.");
		
		var messages = await _conversationRepository.GetMessagesAsync(notification.Chat);
		
		foreach (var message in messages)
		{
			await _chatHub.RestoreMessages(message.UserId, message.UserName, message.Date, message.Text);
		}
	}
}