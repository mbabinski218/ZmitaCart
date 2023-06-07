using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public class GetOffersByCategoriesNameHandler : IRequestHandler<GetOffersByCategoriesNameQuery, Result<List<NamedList<OfferInfoDto>>>>
{
	private readonly IOfferRepository _offerRepository;

	public GetOffersByCategoriesNameHandler(IOfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
	}

	public async Task<Result<List<NamedList<OfferInfoDto>>>> Handle(GetOffersByCategoriesNameQuery request, 
		CancellationToken cancellationToken)
	{
		return await _offerRepository.GetOffersByCategoriesNameAsync(request.CategoriesNames, request.Size);
	}
}