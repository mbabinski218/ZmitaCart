using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Exceptions;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public ConversationRepository(ApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
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

	public async Task<PaginatedList<ConversationInfoDto>> GetConversationsAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		_ = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
		           ?? throw new NotFoundException("User does not exist");

		return await _dbContext.Chats
			.Where(uc => uc.UserId == userId)
			.Include(uc => uc.Conversation)
			.Include(uc => uc.User)
			.ProjectToType<ConversationInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
		
		// if (user.Chats is null) return new PaginatedList<ConversationInfoDto>();
		//
		// return await _dbContext.Users
		// 	.Where(u => u.Id == userId)
		// 	.Include(u => u.Chats)
		// 	!.ThenInclude(uc => uc.Conversation)
		// 	.ThenInclude(c => c.Messages)
		// 	.Include(u => u.Chats)
		// 	!.ThenInclude(uc => uc.Conversation)
		// 	.ThenInclude(c => c.Offer)
		// 	.SelectMany(u => u.Chats ?? new List<UserConversation>())
		// 	.ProjectTo<ConversationInfoDto>(_mapper.ConfigurationProvider)
		// 	.ToPaginatedListAsync(pageNumber, pageSize);
	}

	public Task<IEnumerable<MessageDto>> GetMessagesAsync(string chat)
	{
		// return await _dbContext.Messages
		// 	.Where(m => m.Conversation.Chat == int.Parse(chat))
		// 	.Include(m => m.User)
		// 	.ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
		// 	.ToListAsync();
		
		throw new NotImplementedException();
	}
}