using MediatR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class MessageSentHandler : INotificationHandler<MessageSent>
{
	private readonly IChatHub _chatHub;

	public MessageSentHandler(IChatHub chatHub)
	{
		_chatHub = chatHub;
	}

	public async Task Handle(MessageSent notification, CancellationToken cancellationToken)
	{
		await _chatHub.SendMessage(notification.Message.User.FirstName, notification.Message.ConversationId.ToString(),
			notification.Message.Text, notification.Message.Date, cancellationToken);
	}
}