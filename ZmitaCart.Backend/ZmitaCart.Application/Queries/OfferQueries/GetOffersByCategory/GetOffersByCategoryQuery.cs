using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategory;

public record GetOffersByCategoryQuery : IRequest<IEnumerable<OfferInfoDto>>
{
    public int CategoryId { get; init; }
}