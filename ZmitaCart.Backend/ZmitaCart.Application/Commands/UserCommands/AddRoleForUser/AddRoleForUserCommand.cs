using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.AddRoleForUser;

public record AddRoleForUserCommand : IRequest
{
	public string? UserEmail { get; init; }
	public string? Role { get; init; }
}