using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

public class GetAllConversationsHandler : IRequestHandler<GetAllConversationsQuery, PaginatedList<ConversationInfoDto>>
{
	private readonly IConversationRepository _conversationRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetAllConversationsHandler(IConversationRepository conversationRepository, ICurrentUserService currentUserService)
	{
		_conversationRepository = conversationRepository;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<ConversationInfoDto>> Handle(GetAllConversationsQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId == null)
		{
			throw new UnauthorizedAccessException("User is not logged in");
		}

		var userId = int.Parse(_currentUserService.UserId);
		
		return await _conversationRepository.GetConversationsAsync(userId, request.PageNumber, request.PageSize);
	}
}