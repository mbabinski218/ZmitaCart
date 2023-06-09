using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class CreatedChatHandler : INotificationHandler<CreatedChat>
{
	private readonly IConversationRepository _conversationRepository;

	public CreatedChatHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task Handle(CreatedChat notification, CancellationToken cancellationToken)
	{
		var userId = int.Parse(notification.UserId);
		
		var chatId = await _conversationRepository.IsChatExists(notification.OfferId, userId);
		
		notification.ChatId = chatId ?? await _conversationRepository.CreateConversationAsync(userId, notification.OfferId);
	}
}