using MediatR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Events;

public class MessageSentHandler : INotificationHandler<MessageSent>
{
	private readonly IConversationRepository _conversationRepository;

	public MessageSentHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task Handle(MessageSent notification, CancellationToken cancellationToken)
	{
		var userId = int.Parse(notification.UserId);
		await _conversationRepository.SendMessageAsync(userId, notification.ConversationId, notification.Text);
	}
}