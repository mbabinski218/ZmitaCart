using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.GiveFeedback;

public record GiveFeedbackCommand : IRequest<int>
{
	public int RecipientId { get; set; }
	public int Rating { get; set; }
	public string? Comment { get; set; }
}