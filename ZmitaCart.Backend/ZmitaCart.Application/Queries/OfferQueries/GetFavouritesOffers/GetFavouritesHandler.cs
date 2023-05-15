using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffers;

public class GetFavoritesHandler : IRequestHandler<GetFavouritesOffersQuery, PaginatedList<OfferInfoDto>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetFavoritesHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<OfferInfoDto>> Handle(GetFavouritesOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to get favourites offers.");
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		return await _offerRepository.GetFavoritesOffersAsync(userId, request.PageNumber, request.PageSize);
	}
}