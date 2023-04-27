using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.BuyOffer;

public class BuyOfferHandler : IRequestHandler<BuyOfferCommand>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public BuyOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task Handle(BuyOfferCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to buy an offer.");
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		//TODO Add google pay api call
		
		await _offerRepository.BuyAsync(userId, request.OfferId, request.Quantity);
	}
}