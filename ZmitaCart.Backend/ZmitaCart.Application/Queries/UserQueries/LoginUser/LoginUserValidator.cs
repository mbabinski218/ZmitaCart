using FluentValidation;

namespace ZmitaCart.Application.Queries.UserQueries.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserQuery>
{
	public LoginUserValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Enter email address")
			.EmailAddress().WithMessage("Wrong email format");
		
		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Enter password");
	}
}