using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.SetPhoneNumber;

public class SetPhoneNumberValidator : AbstractValidator<SetPhoneNumberCommand>
{
    public SetPhoneNumberValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Enter phone number")
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$").WithMessage("Invalid phone number");
    }
}