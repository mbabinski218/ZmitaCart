using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;

public class DeleteOfferHandler : IRequestHandler<DeleteOfferCommand>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IPictureRepository _pictureRepository;

	public DeleteOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService, IPictureRepository pictureRepository)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
		_pictureRepository = pictureRepository;
	}

	public async Task Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
	{
		var user = _currentUserService.UserId;
		if (user is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to delete an offer.");
		}

		var userId = int.Parse(user);
		
		await _pictureRepository.RemoveAsync(userId, request.Id);
		await _offerRepository.DeleteAsync(userId, request.Id);
	}
}