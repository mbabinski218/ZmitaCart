using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IConversationRepository
{
	public Task<int> CreateConversationAsync(int userId, int offerId);
	public Task<int> SendMessageAsync(int userId, int conversationId, string text);
	public Task<PaginatedList<ConversationInfoDto>> GetConversationsAsync(int userId, int? pageNumber = null, int? pageSize = null);
	public Task<IEnumerable<MessageDto>> GetMessagesAsync(int chat);
	public Task<IEnumerable<int>> GetConversationUserIds(int chat);
	public Task<bool> IsChatExists(int chat);
}