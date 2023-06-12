using FluentResults;
using ZmitaCart.Application.Dtos.ConversationDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IConversationRepository
{
	public Task<Result<int>> CreateConversationAsync(int offerId, int userId);
	public Task<Result> SendMessageAsync(int userId, int conversationId, DateTimeOffset date, string text);
	public Task<Result<IEnumerable<ConversationInfoDto>>> GetConversationsAsync(int userId);
	public Task<Result<IEnumerable<MessageDto>>> GetMessagesAsync(int chat);
	public Task<Result<IEnumerable<int>>> GetUserConversations(int userId);
	public Task<int> IsChatExists(int offerId, int userId);
	public Task IncrementNotificationStatus(int userId, int chatId);
	public Task DecrementNotificationStatus(int userId, int chatId);
	public Task<Result<int>> ReadNotificationStatus(int userId);
}