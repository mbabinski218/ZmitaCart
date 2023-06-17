using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOfferCreatorConnectionId;

public class GetOfferCreatorConnectionIdHandler : IRequestHandler<GetOfferCreatorConnectionIdQuery, Result<string>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IUserRepository _userRepository;

	public GetOfferCreatorConnectionIdHandler(IOfferRepository offerRepository, IUserRepository userRepository)
	{
		_offerRepository = offerRepository;
		_userRepository = userRepository;
	}

	public async Task<Result<string>> Handle(GetOfferCreatorConnectionIdQuery request, CancellationToken cancellationToken)
	{
		var userId = await _offerRepository.GetUserIdByOfferIdAsync(request.OfferId);
		if (userId.IsFailed)
		{
			return Result.Fail(userId.Errors.ToString());
		}
		
		var connectionId = await _userRepository.GetConnectionIdByUserIdAsync(userId.Value);
		if (connectionId.IsFailed || connectionId.Value is null)
		{
			return Result.Fail(userId.Errors.ToString());
		}

		return connectionId.Value;
	}
}