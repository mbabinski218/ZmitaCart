using FluentResults;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IConversationRepository
{
	public Task<Result<int>> CreateConversationAsync(int offerId, int userId);
	public Task<Result<bool>> SendMessageAsync(int userId, int conversationId, DateTimeOffset date, string text, bool isConnected);
	public Task<Result<IEnumerable<ConversationDto>>> GetConversationsAsync(int userId);
	public Task<Result<IEnumerable<MessageDto>>> GetMessagesAsync(int chat);
	public Task<Result<IEnumerable<int>>> GetUserConversationsAsync(int userId);
	public Task<int> IsChatExistsAsync(int offerId, int userId);
	public Task IncrementNotificationStatusAsync(int userId, int chatId);
	public Task DecrementNotificationStatusAsync(int userId, int chatId);
	public Task<Result<int>> ReadNotificationStatusAsync(int userId);
	public Task<Result<ConversationInfoDto>> GetConversationAsync(int conversationId, int userId);
}