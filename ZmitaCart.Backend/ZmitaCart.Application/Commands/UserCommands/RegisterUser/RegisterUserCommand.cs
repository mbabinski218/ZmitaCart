using MediatR;
namespace ZmitaCart.Application.Commands.UserCommands.RegisterUser;

public record RegisterUserCommand : IRequest
{
	public string Email { get; init; } = null!;
	public string FirstName { get; init; } = null!;
	public string LastName { get; init; } = null!;
	public string Password { get; init; } = null!;
	public string ConfirmedPassword { get; init; } = null!;
}