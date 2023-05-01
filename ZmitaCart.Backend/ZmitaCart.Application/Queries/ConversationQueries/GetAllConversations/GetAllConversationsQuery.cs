using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

public record GetAllConversationsQuery : IRequest<PaginatedList<ConversationInfoDto>>
{
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}