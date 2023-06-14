using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetUserConversations;

public class GetUserConversationsHandler : IRequestHandler<GetUserConversationsQuery, Result<IEnumerable<int>>>
{
	private readonly IConversationRepository _conversationRepository;

	public GetUserConversationsHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task<Result<IEnumerable<int>>> Handle(GetUserConversationsQuery request, CancellationToken cancellationToken)
	{
		return await _conversationRepository.GetUserConversationsAsync(request.UserId);
	}
}