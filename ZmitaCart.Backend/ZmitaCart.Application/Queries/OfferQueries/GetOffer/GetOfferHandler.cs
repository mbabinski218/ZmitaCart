using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffer;

public class GetOfferHandler : IRequestHandler<GetOfferQuery, OfferDto>
{
    private readonly  IOfferRepository _offerRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
    {
        _offerRepository = offerRepository;
        _currentUserService = currentUserService;
    }

    public async Task<OfferDto> Handle(GetOfferQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            return await _offerRepository.GetOfferAsync(request.Id);
        }
		
        var userId = int.Parse(_currentUserService.UserId);
        
        var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
        var offer = await _offerRepository.GetOfferAsync(request.Id);
        
        if (favorites.Contains(offer.Id))
        {
            offer.IsFavourite = true;
        }

        return offer;
    }
}