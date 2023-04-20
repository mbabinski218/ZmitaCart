using AutoMapper;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands;

public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, int>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
	private readonly IPictureRepository _pictureRepository;

	public CreateOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService, IMapper mapper, 
		IPictureRepository pictureRepository)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
		_mapper = mapper;
		_pictureRepository = pictureRepository;
	}

	public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to create an offer.");
		}
		
		var offer = _mapper.Map<CreateOfferDto>(request);
		offer.UserId = int.Parse(_currentUserService.UserId);
		offer.CreatedAt = DateTimeOffset.Now;
		offer.IsAvailable = true;
		
		var offerId = await _offerRepository.CreateAsync(offer);

		if (request.Pictures is not null)
		{
			await _pictureRepository.AddAsync(offerId, request.Pictures);
		}

		return offerId;
	}
}