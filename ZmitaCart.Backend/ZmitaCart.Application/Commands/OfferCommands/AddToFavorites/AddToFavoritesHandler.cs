using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;

public class AddToFavoritesHandler : IRequestHandler<AddToFavoritesCommand>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public AddToFavoritesHandler(ICurrentUserService currentUserService, IOfferRepository offerRepository)
	{
		_currentUserService = currentUserService;
		_offerRepository = offerRepository;
	}

	public async Task Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to add an offer to favorites.");
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		await _offerRepository.AddToFavoritesAsync(userId, request.Id);
	}
}