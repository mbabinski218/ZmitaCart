using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.SetPhoneNumber;

public record SetPhoneNumberCommand : IRequest
{
	public string PhoneNumber { get; init; } = null!;
}