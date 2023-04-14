using MediatR;

namespace ZmitaCart.Application.Commands.WeatherForecastCommands.JoinChat;

public record JoinChatCommand : IRequest
{
	public int UserId { get; init; }
	public int ChatId { get; init; }
}