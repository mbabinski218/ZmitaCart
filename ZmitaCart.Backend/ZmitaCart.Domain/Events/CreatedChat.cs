using MediatR;

namespace ZmitaCart.Domain.Events;

public record CreatedChat(string UserId, int OfferId) : INotification
{
	public int ChatId { get; set; }
}