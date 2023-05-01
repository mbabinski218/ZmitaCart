using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IConversationRepository
{
	public Task<int> CreateConversationAsync(int userId, int offerId);
	public Task<int> SendMessageAsync(int userId, int conversationId, string text);
	public Task<IEnumerable<ConversationInfoDto>> GetConversationsAsync(int userId);
	public Task<IEnumerable<MessageDto>> GetMessagesAsync(int conversationId);
}