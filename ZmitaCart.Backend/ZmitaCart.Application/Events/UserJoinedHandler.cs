using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.Events;

public class UserJoinedHandler : INotificationHandler<UserJoined>
{
	private readonly IUserRepository _userRepository;

	public UserJoinedHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(UserJoined notification, CancellationToken cancellationToken)
	{
		var result = await _userRepository.AddConnectionIdAsync(notification.UserId, notification.ConnectionId);
		
		if (result.IsFailed)
		{
			throw new InvalidDataException(result.Errors.ToString());
		}
	}
}