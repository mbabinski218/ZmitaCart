using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

public record GetAllConversationsQuery(int UserId) : IRequest<Result<IEnumerable<ConversationInfoDto>>>;