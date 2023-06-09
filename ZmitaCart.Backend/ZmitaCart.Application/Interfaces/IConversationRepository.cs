using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IConversationRepository
{
	public Task<Result<int>> CreateConversationAsync(int offerId, int userId);
	public Task<int> SendMessageAsync(int userId, int conversationId, string text);
	public Task<PaginatedList<ConversationInfoDto>> GetConversationsAsync(int userId, int? pageNumber = null, int? pageSize = null);
	public Task<IEnumerable<MessageDto>> GetMessagesAsync(int chat);
	public Task<IEnumerable<int>> GetConversationUserIds(int chat);
	public Task<int> IsChatExists(int offerId, int userId);
}