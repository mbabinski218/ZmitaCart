using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;

public record DeleteOfferCommand : IRequest
{
	public int Id { get; set; }
}