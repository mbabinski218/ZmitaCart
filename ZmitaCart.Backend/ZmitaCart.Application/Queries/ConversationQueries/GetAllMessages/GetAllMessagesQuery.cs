using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllMessages;

public record GetAllMessagesQuery(int Chat, int UserId) : IRequest<Result<IEnumerable<MessageDto>>>;