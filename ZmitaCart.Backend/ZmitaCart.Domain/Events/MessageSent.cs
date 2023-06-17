using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record MessageSent(string UserId, int ChatId, DateTimeOffset Date, string Text, bool IsConnected) : IDomainEvent
{
	public bool FirstMessage { get; set; }
}