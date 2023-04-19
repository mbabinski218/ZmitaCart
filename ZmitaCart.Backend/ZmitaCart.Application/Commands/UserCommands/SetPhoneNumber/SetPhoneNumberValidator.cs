using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.SetPhoneNumber;

public class SetPhoneNumberValidator : AbstractValidator<SetPhoneNumberCommand>
{
	public SetPhoneNumberValidator()
	{
		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Enter phone number")
			.Matches(@"/^\+?[1-9][0-9]{7,14}$/").WithMessage("Invalid phone number");
	}
}