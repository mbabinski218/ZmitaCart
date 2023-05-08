using MediatR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Events;

public class JoinedChatHandler : INotificationHandler<JoinedChat>
{
	private readonly IChatHub _chatHub;
	private readonly IConversationRepository _conversationRepository;

	public JoinedChatHandler(IChatHub chatHub, IConversationRepository conversationRepository)
	{
		_chatHub = chatHub;
		_conversationRepository = conversationRepository;
	}
	
	public async Task Handle(JoinedChat notification, CancellationToken cancellationToken)
	{
		var messages = await _conversationRepository.GetMessagesAsync(notification.Chat);
		
		foreach (var message in messages)
		{
			await _chatHub.RestoreMessagesAsync(message.UserId, message.User, notification.Chat, message.Text, message.Date, cancellationToken);
		}
	}
}