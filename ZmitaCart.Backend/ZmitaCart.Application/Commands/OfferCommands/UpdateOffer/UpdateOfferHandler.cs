using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;

public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, Result<int>>
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

	public async Task<Result<int>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
	{		
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to update an offer."));
		}
		
		var offer = _mapper.Map<UpdateOfferDto>(request);
		offer.UserId = int.Parse(_currentUserService.UserId);
		
		if(request.PicturesToAdd is not null)
		{
			var addPic = await _pictureRepository.AddAsync(offer.UserId, offer.Id, request.PicturesToAdd);
			if (addPic.IsFailed)
			{
				return Result.Fail(addPic.Errors);
			}
		}
		
		if(request.PictureIdsToRemove is not null)
		{
			var removePic = await _pictureRepository.RemoveAsync(offer.UserId, offer.Id, request.PictureIdsToRemove);
			if (removePic.IsFailed)
			{
				return Result.Fail(removePic.Errors);
			}
		}
		
		return await _offerRepository.UpdateAsync(offer);
	}
}