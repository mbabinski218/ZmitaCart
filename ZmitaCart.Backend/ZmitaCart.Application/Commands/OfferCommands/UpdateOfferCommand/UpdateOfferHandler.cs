using AutoMapper;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.UpdateOfferCommand;

public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, int>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IMapper _mapper;
	private readonly IPictureRepository _pictureRepository;
	private readonly ICurrentUserService _currentUserService;
	
	public UpdateOfferHandler(IOfferRepository offerRepository, IMapper mapper, IPictureRepository pictureRepository, 
		ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_mapper = mapper;
		_pictureRepository = pictureRepository;
		_currentUserService = currentUserService;
	}

	public async Task<int> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
	{		
		if (_currentUserService.UserId is null)
		{
			throw new UnauthorizedAccessException("You are not authorized to update an offer.");
		}
		
		var offer = _mapper.Map<UpdateOfferDto>(request);
		offer.UserId = int.Parse(_currentUserService.UserId);
		
		if(request.PicturesToAdd is not null)
		{
			await _pictureRepository.AddAsync(offer.UserId, offer.Id, request.PicturesToAdd);
		}
		
		if(request.PictureIdsToRemove is not null)
		{
			await _pictureRepository.RemoveAsync(offer.UserId, offer.Id, request.PictureIdsToRemove);
		}
		
		return await _offerRepository.UpdateAsync(offer);
	}
}