using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public record GetOffersByCategoriesNameQuery(List<string> CategoriesNames, int Size) 
	: IRequest<Result<Dictionary<string, List<OfferInfoDto>>>>;