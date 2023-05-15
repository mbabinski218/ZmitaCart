using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;

public class ExternalAuthenticationValidation : AbstractValidator<ExternalAuthenticationCommand>
{
	public ExternalAuthenticationValidation()
	{
		RuleFor(e => e.IdToken)
			.NotNull().WithMessage("Failed to authenticate user")
			.NotEmpty().WithMessage("Failed to authenticate user");

		RuleFor(x => x.Provider)
			.NotNull().WithMessage("Failed to authenticate user")
			.NotEmpty().WithMessage("Failed to authenticate user");
	}
}