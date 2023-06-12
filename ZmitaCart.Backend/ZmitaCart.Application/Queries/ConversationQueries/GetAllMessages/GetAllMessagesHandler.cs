using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllMessages;

public class GetAllMessagesHandler : IRequestHandler<GetAllMessagesQuery, Result<IEnumerable<MessageDto>>> 
{
	private readonly IConversationRepository _conversationRepository;

	public GetAllMessagesHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task<Result<IEnumerable<MessageDto>>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
	{
		var messages =  await _conversationRepository.GetMessagesAsync(request.Chat);
		if (messages.IsFailed)
		{
			return Result.Fail<IEnumerable<MessageDto>>(messages.Errors.ToString());
		}

		await _conversationRepository.DecrementNotificationStatus(request.UserId, request.Chat);

		return messages;
	}
}