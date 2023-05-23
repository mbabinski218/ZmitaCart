using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;

public record AddToFavoritesCommand : IRequest<Result>
{
	public int Id { get; init; }
}