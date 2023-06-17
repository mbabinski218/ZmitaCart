using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class JoinedChatHandler : INotificationHandler<JoinedChat>
{
	private readonly IUserRepository _userRepository;

	public JoinedChatHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(JoinedChat notification, CancellationToken cancellationToken)
	{
		await _userRepository.SetCurrentChatAsync(notification.UserId, notification.ChatId);
	}
}