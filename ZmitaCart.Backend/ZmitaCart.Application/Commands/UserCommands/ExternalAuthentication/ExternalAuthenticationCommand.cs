using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;

public record ExternalAuthenticationCommand : IRequest<Result<string>>
{
	public string? Provider { get; init; }
	public string? IdToken { get; init; }
}