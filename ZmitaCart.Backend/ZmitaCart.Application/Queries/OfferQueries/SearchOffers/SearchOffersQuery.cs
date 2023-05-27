using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public record SearchOffersQuery : IRequest<Result<PaginatedList<OfferInfoDto>>>
{
	public string? Title { get; init; }
	public int? CategoryId { get; init; }
	public string? Condition { get; init; }
	public decimal? MinPrice { get; init; }
	public decimal? MaxPrice { get; init; }
	public bool? PriceAscending { get; init; }
	public bool? CreatedAscending { get; init; }
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}