using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetConversation;

public record GetConversationQuery(int Id, int UserId) : IRequest<Result<ConversationInfoDto>>;