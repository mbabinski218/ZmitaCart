using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOfferCommand;

public record DeleteOfferCommand : IRequest
{
	public int Id { get; set; }
}