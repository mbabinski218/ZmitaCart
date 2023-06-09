using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.ConversationCommands.CreateConversation;

public record CreateConversationCommand(int OfferId) : IRequest<Result<int>>;