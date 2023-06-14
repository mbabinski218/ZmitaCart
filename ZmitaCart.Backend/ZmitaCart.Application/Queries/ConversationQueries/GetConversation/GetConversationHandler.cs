using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetConversation;

public class GetConversationHandler : IRequestHandler<GetConversationQuery, Result<ConversationInfoDto>>
{
	private readonly IConversationRepository _conversationRepository;

	public GetConversationHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task<Result<ConversationInfoDto>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
	{
		return await _conversationRepository.GetConversationAsync(request.Id, request.UserId);
	}
}