using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.BuyOffer;

public record BuyOfferCommand : IRequest
{
	public int OfferId { get; init; }
	public int Quantity { get; init; }
}