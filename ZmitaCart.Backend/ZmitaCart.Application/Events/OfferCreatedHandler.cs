using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class OfferCreatedHandler : INotificationHandler<OfferCreated>
{
	private readonly IPictureRepository _pictureRepository;

	public OfferCreatedHandler(IPictureRepository pictureRepository)
	{
		_pictureRepository = pictureRepository;
	}

	public async Task Handle(OfferCreated notification, CancellationToken cancellationToken)
	{
		if (notification.FileStreams is not null)
		{
			await _pictureRepository.AddAsync(notification.Offer.UserId, notification.Offer.Id, notification.FileStreams);
		}
	}
}