using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOfferCreatorConnectionId;

public record GetOfferCreatorConnectionIdQuery(int OfferId) : IRequest<Result<string>>;