using MediatR;
using ZmitaCart.API.Hubs;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.API.Handlers;

public class JoinedChatEventHandler : INotificationHandler<JoinedChatEvent>
{
	private readonly ChatHub _hubContext = new();

	public async Task Handle(JoinedChatEvent notification, CancellationToken cancellationToken)
	{
		await _hubContext.JoinGroup(notification.ChatId, cancellationToken);
	}
}