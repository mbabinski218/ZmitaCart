using MediatR;

namespace ZmitaCart.Application.Events;

public record CreatedChat(string UserId, int OfferId) : INotification
{
	public int ChatId { get; set; }
}