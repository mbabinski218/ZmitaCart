using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetUserConversations;

public record GetUserConversationsQuery(int UserId) : IRequest<Result<IEnumerable<int>>>;