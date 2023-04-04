using MediatR;

namespace ZmitaCart.Domain.Events;

public class JoinedChatEvent : INotification
{
	public int UserId { get; init; }
	public int ChatId { get; init; }
}