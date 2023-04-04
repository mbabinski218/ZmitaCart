using MediatR;

namespace ZmitaCart.Application.WeatherForecasts.Commands.JoinChat;

public record JoinChatCommand : IRequest
{
	public int UserId { get; init; }
	public int ChatId { get; init; }
}