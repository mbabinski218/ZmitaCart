using FluentValidation;

namespace ZmitaCart.Application.Commands.ConversationCommands.SendMessage;

public class SendMessageValidator : AbstractValidator<SendMessageCommand>
{
	public SendMessageValidator()
	{
		RuleFor(x => x.ConversationId)
			.NotNull().WithMessage("ConversationId is required");

		RuleFor(x => x.Text)
			.NotNull().WithMessage("Text is required")
			.MinimumLength(1).WithMessage("Text is required")
			.MaximumLength(2000).WithMessage("Maximum length of text is 1000");
	}
}