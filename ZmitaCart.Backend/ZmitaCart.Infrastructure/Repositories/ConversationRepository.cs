using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
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
			User = user
		};
		
		var ownerConversation = new UserConversation
		{
			ConversationId = conversation.Id,
			Conversation = conversation,
			UserId = offer.UserId,
			User = offer.User
		};
		
		await _dbContext.Conversations.AddAsync(conversation);
		await _dbContext.Chats.AddRangeAsync(userConversation, ownerConversation);
		await _dbContext.SaveChangesAsync();
		
		return conversation.Id;
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

	public async Task<PaginatedList<ConversationInfoDto>> GetConversationsAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		_ = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
		    ?? throw new NotFoundException("User does not exist");

		var conversation = await _dbContext.Conversations
			.Include(c => c.UserConversations)
			.Include(c => c.Messages)
			.Where(c => c.UserConversations.Any(uc => uc.UserId == userId))
			.Where(c => c.Messages.Any())
			.Include(c => c.Offer)
				.ThenInclude(o => o.Pictures)
			.ProjectToType<ConversationInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
		
		foreach (var c in conversation.Items)
		{
			var user = (await _dbContext.Chats
				.Include(ch => ch.User)
				.FirstAsync(ch => ch.ConversationId == c.Id && ch.UserId != userId)).User;

			c.WithUser = $"{user.FirstName} {user.LastName}";
		}

		return conversation;
		
		// var chats =  await _dbContext.Chats
		// 	.Where(uc => uc.UserId == userId)
		// 	.Include(uc => uc.Conversation)
		// 	.Include(uc => uc.User)
		// 	.ProjectToType<ConversationInfoDto>()
		// 	.ToPaginatedListAsync(pageNumber, pageSize);
		//
		// foreach (var chat in chats.Items)
		// {
		// 	chat.WithUser = await _dbContext.Chats
		// 		.Where(uc => uc.ConversationId == chat.ConversationId && uc.UserId != userId)
		// 		.Include(uc => uc.User)
		// 		.Select(uc => uc.User.FirstName + " " + uc.User.LastName)
		// 		.FirstAsync();
		// }

		// return chats;
	}

	public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int chat)
	{
		return await _dbContext.Conversations
			.Where(c => c.Id == chat && c.Messages.Any())
			.SelectMany(c => c.Messages)
			.ProjectToType<MessageDto>()
			.ToListAsync();
	}

	public async Task<IEnumerable<int>> GetConversationUserIds(int chat)
	{
		return await _dbContext.Chats
			.Where(uc => uc.ConversationId == chat)
			.Select(uc => uc.UserId)
			.ToListAsync();
	}

	public async Task<int> IsChatExists(int offerId, int userId)
	{
		return await _dbContext.Chats
			.Include(c => c.Conversation)
			.Where(c => c.UserId == userId && c.Conversation.OfferId == offerId)
			.Select(c => c.ConversationId)
			.FirstOrDefaultAsync();
	}
}