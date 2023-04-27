using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategory;

public class GetOffersByCategoryHandler : IRequestHandler<GetOffersByCategoryQuery, PaginatedList<OfferInfoDto>>
{
    private readonly IOfferRepository _offerRepository;

    public GetOffersByCategoryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<PaginatedList<OfferInfoDto>> Handle(GetOffersByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _offerRepository.GetOffersByCategoryAsync(request.CategoryId, request.PageNumber, request.PageSize);
    }
}