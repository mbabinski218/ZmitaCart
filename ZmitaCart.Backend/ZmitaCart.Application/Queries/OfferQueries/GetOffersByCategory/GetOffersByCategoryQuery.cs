using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategory;

public record GetOffersByCategoryQuery : IRequest<PaginatedList<OfferInfoDto>>
{
    public int CategoryId { get; init; }
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}