using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

public class GetAllConversationsHandler : IRequestHandler<GetAllConversationsQuery, Result<IEnumerable<ConversationDto>>>
{
	private readonly IConversationRepository _conversationRepository;

	public GetAllConversationsHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task<Result<IEnumerable<ConversationDto>>> Handle(GetAllConversationsQuery request, CancellationToken cancellationToken)
	{
		return await _conversationRepository.GetConversationsAsync(request.UserId);
	}
}