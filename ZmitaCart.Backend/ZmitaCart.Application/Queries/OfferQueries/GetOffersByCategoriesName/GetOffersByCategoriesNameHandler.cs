using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public class GetOffersByCategoriesNameHandler : IRequestHandler<GetOffersByCategoriesNameQuery, Result<Dictionary<string, List<OfferInfoDto>>>>
{
	private readonly IOfferRepository _offerRepository;

	public GetOffersByCategoriesNameHandler(IOfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
	}

	public async Task<Result<Dictionary<string, List<OfferInfoDto>>>> Handle(GetOffersByCategoriesNameQuery request, 
		CancellationToken cancellationToken)
	{
		return await _offerRepository.GetOffersByCategoriesNameAsync(request.CategoriesNames, request.Size);
	}
}