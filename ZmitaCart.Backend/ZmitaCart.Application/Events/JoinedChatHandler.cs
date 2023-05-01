using MediatR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class JoinedChatHandler : INotificationHandler<JoinedChat>
{
	private readonly IChatHub _chatHub;

	public JoinedChatHandler(IChatHub chatHub)
	{
		_chatHub = chatHub;
	}

	public async Task Handle(JoinedChat notification, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}