using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetConversation;

public class GetConversationHandler : IRequestHandler<GetConversationQuery, Result<ConversationInfoDto>>
{
	private readonly IConversationRepository _conversationRepository;
	private readonly IUserRepository _userRepository;

	public GetConversationHandler(IConversationRepository conversationRepository, IUserRepository userRepository)
	{
		_conversationRepository = conversationRepository;
		_userRepository = userRepository;
	}

	public async Task<Result<ConversationInfoDto>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
	{
		var conversation = await _conversationRepository.GetConversationAsync(request.Id, request.UserId);

		if (conversation.IsFailed)
		{
			return Result.Fail(conversation.Errors.ToList());
		}
		
		var withUserConnectedChatId = await _userRepository.GetCurrentChatAsync(conversation.Value.WithUserId);
		
		if(withUserConnectedChatId.IsFailed)
		{
			return Result.Fail(withUserConnectedChatId.Errors.ToList());
		}
		
		conversation.Value.WithUserConnectedChatId = withUserConnectedChatId.Value;

		return conversation;
	}
}