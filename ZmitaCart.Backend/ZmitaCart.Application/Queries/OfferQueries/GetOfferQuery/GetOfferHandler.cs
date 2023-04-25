using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOfferQuery;

public class GetOfferHandler : IRequestHandler<GetOfferQuery, OfferDto>
{
    private readonly  IOfferRepository _offerRepository;

    public GetOfferHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<OfferDto> Handle(GetOfferQuery request, CancellationToken cancellationToken)
    {
        return await _offerRepository.GetOfferAsync(request.Id);
    }
}