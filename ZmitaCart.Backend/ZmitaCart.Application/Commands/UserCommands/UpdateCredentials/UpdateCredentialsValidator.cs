using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateCredentials;

public class UpdateCredentialsValidator : AbstractValidator<UpdateCredentialsCommand>
{
    public UpdateCredentialsValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$").WithMessage("Invalid phone number");
    }
}