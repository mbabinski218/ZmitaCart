using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public class SearchOffersHandler : IRequestHandler<SearchOffersQuery, PaginatedList<OfferInfoDto>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public SearchOffersHandler(IOfferRepository offerRepository, IMapper mapper, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<OfferInfoDto>> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
	{
		var dto = _mapper.Map<SearchOfferDto>(request);
		
		if (_currentUserService.UserId is null)
		{
			return await _offerRepository.SearchOffersAsync(dto, request.PageNumber, request.PageSize); 
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
		var offers = await _offerRepository.SearchOffersAsync(dto, request.PageNumber, request.PageSize);

		offers.Items.ForEach(o =>
		{
			o.IsFavourite = favorites.Contains(o.Id);
		});
		
		return offers;
	}
}