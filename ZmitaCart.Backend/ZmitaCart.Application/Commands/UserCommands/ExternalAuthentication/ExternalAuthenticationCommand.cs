using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;

public record ExternalAuthenticationCommand : IRequest<string>
{
	public string? Provider { get; init; }
	public string? IdToken { get; init; }
}