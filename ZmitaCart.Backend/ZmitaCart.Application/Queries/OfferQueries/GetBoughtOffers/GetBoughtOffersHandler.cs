using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;

public class GetBoughtOffersHandler : IRequestHandler<GetBoughtOffersQuery, Result<PaginatedList<BoughtOfferDto>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetBoughtOffersHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<PaginatedList<BoughtOfferDto>>> Handle(GetBoughtOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to get bought offers."));
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		return await _offerRepository.GetBoughtAsync(userId, request.PageNumber, request.PageSize);
	}
}