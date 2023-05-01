using MediatR;

namespace ZmitaCart.Application.Commands.ConversationCommands.SendMessage;

public record SendMessageCommand(int ConversationId, string Text) : IRequest<int>;