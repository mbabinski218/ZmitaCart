using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
	private readonly ApplicationDbContext _dbContext;

	public ConversationRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Result<int>> CreateConversationAsync(int offerId, int userId)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

		if (user is null)
		{
			return Result.Fail(new UnauthorizedError("User does not exist"));
		}

		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId);

		if (offer is null)
		{
			return Result.Fail(new UnauthorizedError("Offer does not exist"));
		}

		var conversation = new Conversation
		{
			OfferId = offerId,
			Offer = offer
		};

		var userConversation = new UserConversation
		{
			ConversationId = conversation.Id,
			Conversation = conversation,
			UserId = userId,
			User = user,
			IsRead = true
		};

		var ownerConversation = new UserConversation
		{
			ConversationId = conversation.Id,
			Conversation = conversation,
			UserId = offer.UserId,
			User = offer.User,
			IsRead = false
		};

		await _dbContext.Conversations.AddAsync(conversation);
		await _dbContext.Chats.AddRangeAsync(userConversation, ownerConversation);
		await _dbContext.SaveChangesAsync();

		return conversation.Id;
	}

	public async Task<Result<bool>> SendMessageAsync(int userId, int conversationId, DateTimeOffset date, string text, bool isConnected)
	{
		// user
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
		if (user is null)
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		// conversation
		var conversation = await _dbContext.Conversations
			.Include(c => c.Messages)
			.FirstOrDefaultAsync(c => c.Id == conversationId);
		if (conversation is null)
		{
			return Result.Fail(new NotFoundError("Conversation does not exist"));
		}

		// other user chat
		var otherChat = await _dbContext.Chats
			.FirstOrDefaultAsync(ch => ch.ConversationId == conversationId && ch.UserId != userId);
		if (otherChat is null)
		{
			return Result.Fail(new NotFoundError("Chat does not exist"));
		}
		
		otherChat.IsRead = isConnected;
		
		// user chat
		var myChat = await _dbContext.Chats
			.FirstOrDefaultAsync(ch => ch.ConversationId == conversationId && ch.UserId == userId);
		if (myChat is null)
		{
			return Result.Fail(new NotFoundError("Chat does not exist"));
		}
		
		myChat.IsRead = true;
		
		// is first message
		var firstMessage = !conversation.Messages.Any();
		
		// new message
		var message = new Message
		{
			Text = text,
			UserId = userId,
			User = user,
			ConversationId = conversationId,
			Conversation = conversation,
			Date = date
		};

		// save
		await _dbContext.Messages.AddAsync(message);
		await _dbContext.SaveChangesAsync();

		return firstMessage;
	}

	public async Task<Result<IEnumerable<ConversationDto>>> GetConversationsAsync(int userId)
	{
		if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		var conversations = await _dbContext.Conversations
			.Include(c => c.UserConversations)
			.Include(c => c.Messages)
			.Where(c => c.UserConversations.Any(uc => uc.UserId == userId))
			.Where(c => c.Messages.Any())
			.Include(c => c.Offer)
			.ThenInclude(o => o.Pictures)
			.ProjectToType<ConversationDto>()
			.ToListAsync();

		foreach (var conversation in conversations)
		{
			var user = (await _dbContext.Chats
				.Include(ch => ch.User)
				.FirstAsync(ch => ch.ConversationId == conversation.Id && ch.UserId != userId)).User;

			conversation.WithUser = $"{user.FirstName} {user.LastName}";
			
			conversation.IsRead = await _dbContext.Chats
				.Where(ch => ch.ConversationId == conversation.Id && ch.UserId == userId)
				.Select(ch => ch.IsRead)
				.FirstOrDefaultAsync();
		}

		return conversations;
	}

	public async Task<Result<IEnumerable<MessageDto>>> GetMessagesAsync(int chat)
	{
		return await _dbContext.Conversations
			.Where(c => c.Id == chat && c.Messages.Any())
			.SelectMany(c => c.Messages)
			.ProjectToType<MessageDto>()
			.ToListAsync();
	}

	public async Task<Result<IEnumerable<int>>> GetUserConversationsAsync(int userId)
	{
		return await _dbContext.Chats
			.Where(uc => uc.UserId == userId)
			.Select(uc => uc.ConversationId)
			.ToListAsync();
	}

	public async Task<int> IsChatExistsAsync(int offerId, int userId)
	{
		return await _dbContext.Chats
			.Include(c => c.Conversation)
			.Where(c => c.UserId == userId && c.Conversation.OfferId == offerId)
			.Select(c => c.ConversationId)
			.FirstOrDefaultAsync();
	}

	public async Task IncrementNotificationStatusAsync(int userId, int chatId)
	{
		var chat = await _dbContext.Chats
			.FirstOrDefaultAsync(c => c.UserId == userId && c.ConversationId == chatId);

		if (chat is not null)
		{
			chat.IsRead = false;
		}
		
		await _dbContext.SaveChangesAsync();
	}
	
	public async Task DecrementNotificationStatusAsync(int userId, int chatId)
	{
		var chat = await _dbContext.Chats
			.FirstOrDefaultAsync(c => c.UserId == userId && c.ConversationId == chatId);

		if (chat is not null)
		{
			chat.IsRead = true;
		}
		
		await _dbContext.SaveChangesAsync();
	}

	public async Task<Result<int>> ReadNotificationStatusAsync(int userId)
	{
		if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		var status = 0;
		await _dbContext.Chats
			.Where(c => c.UserId == userId)
			.ForEachAsync(c =>
			{
				if (!c.IsRead) status++;
			});

		return status;
	}

	public async Task<Result<ConversationInfoDto>> GetConversationAsync(int conversationId, int userId)
	{
		if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		var conversation = await _dbContext.Conversations
			.Include(c => c.UserConversations)
			.Include(c => c.Messages)
			.Where(c => c.Id == conversationId)
			.Include(c => c.Offer)
			.ThenInclude(o => o.Pictures)
			.ProjectToType<ConversationInfoDto>()
			.FirstOrDefaultAsync();

		if (conversation is null)
		{
			return Result.Fail(new NotFoundError("Conversation does not exist"));
		} 

		var user = (await _dbContext.Chats
			.Include(ch => ch.User)
			.FirstAsync(ch => ch.ConversationId == conversation.Id && ch.UserId != userId)).User;

		conversation.WithUser = $"{user.FirstName} {user.LastName}";
		
		conversation.IsRead = await _dbContext.Chats
			.Where(ch => ch.ConversationId == conversation.Id && ch.UserId == userId)
			.Select(ch => ch.IsRead)
			.FirstOrDefaultAsync();
		
		return conversation;
	}

	public async Task<Result<int>> GetOtherUserIdOfConversation(int chatId, int userId)
	{
		return await _dbContext.Chats
			.Where(c => c.ConversationId == chatId && c.UserId != userId)
			.Select(c => c.UserId)
			.FirstOrDefaultAsync();
	}
}