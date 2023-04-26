using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;

public record AddToFavoritesCommand : IRequest
{
	public int Id { get; init; }
}