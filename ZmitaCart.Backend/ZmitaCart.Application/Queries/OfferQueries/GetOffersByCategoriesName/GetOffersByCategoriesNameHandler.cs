using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public class GetOffersByCategoriesNameHandler : IRequestHandler<GetOffersByCategoriesNameQuery, Result<IEnumerable<OfferInfoWithCategoryNameDto>>>
{
	private readonly IOfferRepository _offerRepository;

	public GetOffersByCategoriesNameHandler(IOfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
	}

	public async Task<Result<IEnumerable<OfferInfoWithCategoryNameDto>>> Handle(GetOffersByCategoriesNameQuery request, CancellationToken cancellationToken)
	{
		return await _offerRepository.GetOffersByCategoriesNameAsync(request.CategoriesNames, request.Size);
	}
}