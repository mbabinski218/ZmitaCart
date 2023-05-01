using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Exceptions;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
	private readonly ApplicationDbContext _dbContext;

	public ConversationRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<int> CreateConversationAsync(int userId, int offerId)
	{
		throw new NotImplementedException();
	}

	public async Task<int> SendMessageAsync(int userId, int conversationId, string text)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
		           ?? throw new NotFoundException("User not found");

		var conversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId)
		                   ?? throw new NotFoundException("Conversation not found");

		var message = Message.Create(userId, user, conversationId, conversation, text);

		await _dbContext.Messages.AddAsync(message);
		await _dbContext.SaveChangesAsync();
		
		return message.Id;
	}

	public Task<IEnumerable<ConversationInfoDto>> GetConversationsAsync(int userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<MessageDto>> GetMessagesAsync(int conversationId)
	{
		throw new NotImplementedException();
	}
}