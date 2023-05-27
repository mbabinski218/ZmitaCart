using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;

public record DeleteOfferCommand : IRequest<Result>
{
	public int Id { get; set; }
}