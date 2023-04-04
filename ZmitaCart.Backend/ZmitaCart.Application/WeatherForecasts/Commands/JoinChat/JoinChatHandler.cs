using MediatR;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.Application.WeatherForecasts.Commands.JoinChat;

public class JoinChatHandler : IRequestHandler<JoinChatCommand>
{
	private readonly IMediator _mediator;

	public JoinChatHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Handle(JoinChatCommand request, CancellationToken cancellationToken)
	{
		await _mediator.Publish(new JoinedChatEvent
		{
			UserId = request.UserId,
			ChatId = request.ChatId
		}, cancellationToken);
	}
}