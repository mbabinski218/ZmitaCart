using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public class SearchOffersHandler : IRequestHandler<SearchOffersQuery, PaginatedList<OfferInfoDto>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IMapper _mapper;

	public SearchOffersHandler(IOfferRepository offerRepository, IMapper mapper)
	{
		_offerRepository = offerRepository;
		_mapper = mapper;
	}

	public async Task<PaginatedList<OfferInfoDto>> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
	{
		var dto = _mapper.Map<SearchOfferDto>(request);
		
		return await _offerRepository.SearchOffersAsync(dto, request.PageNumber, request.PageSize); 
	}
}