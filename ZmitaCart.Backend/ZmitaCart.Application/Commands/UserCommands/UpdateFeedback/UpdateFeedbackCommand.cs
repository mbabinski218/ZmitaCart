using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;

public record UpdateFeedbackCommand : IRequest<int>
{
	public int  Id { get; set; }
	public int? Rating { get; set; }
	public string? Comment { get; set; }
}