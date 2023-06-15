using MediatR;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.ConversationQueries.ReadNotificationsStatus;

public class ReadNotificationStatusHandler : IRequestHandler<ReadNotificationStatusQuery, int>
{
	private readonly IConversationRepository _conversationRepository;

	public ReadNotificationStatusHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task<int> Handle(ReadNotificationStatusQuery request, CancellationToken cancellationToken)
	{
		var status = await _conversationRepository.ReadNotificationStatusAsync(request.UserId);
		
		return status.IsFailed ? -1 : status.Value;
	}
}