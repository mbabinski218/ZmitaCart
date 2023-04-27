using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;

public class GetBoughtOffersHandler : IRequestHandler<GetBoughtOffersQuery, PaginatedList<BoughtOfferDto>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetBoughtOffersHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<BoughtOfferDto>> Handle(GetBoughtOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to get bought offers.");
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		return await _offerRepository.GetBoughtAsync(userId, request.PageNumber, request.PageSize);
	}
}