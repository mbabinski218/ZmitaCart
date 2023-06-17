using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class MessageSentHandler : INotificationHandler<MessageSent>
{
	private readonly IConversationRepository _conversationRepository;

	public MessageSentHandler(IConversationRepository conversationRepository)
	{
		_conversationRepository = conversationRepository;
	}

	public async Task Handle(MessageSent notification, CancellationToken cancellationToken)
	{
		var userId = int.Parse(notification.UserId);
		
		var result = await _conversationRepository.SendMessageAsync(userId, notification.ChatId, notification.Date, 
			notification.Text, notification.IsConnected);
		
		if (result.IsFailed)
		{
			throw new InvalidDataException(result.Errors.ToString());
		}

		notification.OfferId = result.Value.Item1;
		notification.FirstMessage = result.Value.Item2;
		
		if (!notification.IsConnected)
		{
			await _conversationRepository.IncrementNotificationStatusAsync(userId, notification.ChatId);
		}
	}
}