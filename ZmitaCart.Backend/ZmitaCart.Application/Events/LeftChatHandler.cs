using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class LeftChatHandler : INotificationHandler<LeftChat>
{
	private readonly IUserRepository _userRepository;

	public LeftChatHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(LeftChat notification, CancellationToken cancellationToken)
	{
		await _userRepository.SetCurrentChatAsync(notification.UserId, null);
	}
}