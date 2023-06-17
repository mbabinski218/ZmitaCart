using MediatR;
using Microsoft.AspNetCore.SignalR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllMessages;
using ZmitaCart.Application.Queries.ConversationQueries.GetConversation;
using ZmitaCart.Application.Queries.ConversationQueries.GetUserConversations;
using ZmitaCart.Application.Queries.ConversationQueries.ReadNotificationsStatus;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.API.Hubs;

// TODO zrobić wszystko WSZYSTKO lepiej xd
/// <summary>
/// NIE POLECAM TEGO KODU
/// ALE DZIAŁA
/// Takie sphagetthi że cała rodzina się naje, ale już trochę lepiej
/// </summary>
public class ChatHub : Hub
{
	private readonly IMediator _mediator;
	private readonly IUserRepository _userRepository;
	private readonly IConversationRepository _conversationRepository;

	public ChatHub(IMediator mediator, IUserRepository userRepository, IConversationRepository conversationRepository)
	{
		_mediator = mediator;
		_userRepository = userRepository;
		_conversationRepository = conversationRepository;
	}

	public async Task Join(string user)
	{
		var userId = int.Parse(user);
		var conversations = await _mediator.Send(new GetUserConversationsQuery(userId));
		if (conversations.IsFailed)
		{
			throw new ArgumentException(conversations.Errors[0].ToString());
		}

		foreach (var conversation in conversations.Value)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, conversation.ToString());
		}

		await _mediator.Publish(new UserJoined(userId, Context.ConnectionId));
		
		await ReadNotificationStatus(userId);
	}

	public async Task RestoreAllConversations(string user)
	{
		var userId = int.Parse(user);
		var conversations = await _mediator.Send(new GetAllConversationsQuery(userId));
		if (conversations.IsFailed)
		{
			throw new ArgumentException(conversations.Errors[0].ToString());
		}

		foreach (var conversation in conversations.Value)
		{
			await RestoreConversation(conversation);

			if (conversation.LastMessage is not null)
			{
				await RestoreMessage(conversation.LastMessage);
			}
		}

		await ReadNotificationStatus(userId);
	}

	public async Task RestoreMessages(int chat, string user)
	{
		var userId = int.Parse(user);
		
		var conversation = await _mediator.Send(new GetConversationQuery(chat, userId));
		if (conversation.IsFailed)
		{
			throw new ArgumentException(conversation.Errors[0].ToString());
		}

		var messages = await _mediator.Send(new GetAllMessagesQuery(chat, userId));
		if (messages.IsFailed)
		{
			throw new ArgumentException(messages.Errors[0].ToString());
		}

		foreach (var message in messages.Value)
		{
			await RestoreMessage(message);
		}
		
		await _mediator.Publish(new JoinedChat(userId, chat));

		await ReadNotificationStatus(userId);
		
		if (messages.Value.Any())
		{
			await UpdateConversation(conversation.Value, messages.Value.Last().Date, messages.Value.Last().Text, true);
		}
	}
	
	public async Task LeaveChat(string user)
	{
		var userId = int.Parse(user);
		await _mediator.Publish(new LeftChat(userId));
	}

	public async Task SendMessage(int chat, string user, string userName, string text)
	{
		var userId = int.Parse(user);
		var date = DateTimeOffset.Now;
		
		var conversation = await _mediator.Send(new GetConversationQuery(chat, userId));
		if (conversation.IsFailed)
		{
			throw new ArgumentException(conversation.Errors[0].ToString());
		}

		//TODO wrzucić wszystko co poniżej do GetConversationQuery
		var otherUserId = await _conversationRepository.GetOtherUserIdOfConversation(chat, userId);
		if (otherUserId.IsFailed)
		{
			throw new ArgumentException(otherUserId.Errors[0].ToString());
		}
		
		var otherUserConnectedChatId = await _userRepository.GetCurrentChatAsync(otherUserId.Value);
		if (otherUserConnectedChatId.IsFailed)
		{
			throw new ArgumentException(otherUserConnectedChatId.Errors[0].ToString());
		}
		
		var otherUserConnectionId = await _userRepository.GetConnectionIdByUserIdAsync(otherUserId.Value);
		if (otherUserConnectionId.IsFailed)
		{
			throw new ArgumentException(otherUserConnectionId.Errors[0].ToString());
		}
		
		var messageSentEvent = new MessageSent(user, chat, date, text, otherUserConnectedChatId.Value == chat);
		await _mediator.Publish(messageSentEvent);

		if (messageSentEvent.FirstMessage)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, chat.ToString());

			if (otherUserConnectionId.Value is not null)
			{
				await Groups.AddToGroupAsync(otherUserConnectionId.Value, chat.ToString());
			}
		}

		await NewMessage(chat, userId, userName, date, text);
		
		if (otherUserConnectedChatId.Value == chat)
		{
			await UpdateConversation(conversation.Value, date, text, true);

			if (otherUserConnectionId.Value is not null)
			{
				var otherUserConversation = await _mediator.Send(new GetConversationQuery(chat, otherUserId.Value));
				if (otherUserConversation.IsFailed)
				{
					throw new ArgumentException(otherUserConnectionId.Errors[0].ToString());
				}
				
				await UpdateConversation(otherUserConnectionId.Value, otherUserConversation.Value, date, text, true);
			}
		}
		else
		{
			await UpdateConversation(conversation.Value, date, text, true);

			if (otherUserConnectionId.Value is not null)
			{
				var otherUserConversation = await _mediator.Send(new GetConversationQuery(chat, otherUserId.Value));
				if (otherUserConversation.IsFailed)
				{
					throw new ArgumentException(otherUserConnectionId.Errors[0].ToString());
				}
				
				await UpdateConversation(otherUserConnectionId.Value, otherUserConversation.Value, date, text, false);
				await ReadNotificationStatus(otherUserConnectionId.Value, otherUserId.Value);
			}
		}
	}

	private async Task ReadNotificationStatus(int chatId, int userId)
	{
		var status = await _mediator.Send(new ReadNotificationStatusQuery(userId));
		await Clients.Group(chatId.ToString()).SendAsync("ReceiveNotificationStatus", status);
	}

	private async Task ReadNotificationStatus(int userId)
	{
		var status = await _mediator.Send(new ReadNotificationStatusQuery(userId));
		await Clients.Caller.SendAsync("ReceiveNotificationStatus", status);
	}
	
	private async Task ReadNotificationStatus(string connectionId, int userId)
	{
		var status = await _mediator.Send(new ReadNotificationStatusQuery(userId));
		await Clients.Client(connectionId).SendAsync("ReceiveNotificationStatus", status);
	}

	private async Task RestoreMessage(MessageDto message)
	{
		await Clients.Caller.SendAsync("ReceiveMessage", message.ChatId, message.UserId, message.UserName,
			message.Date, message.Text);
	}

	private async Task RestoreConversation(ConversationDto conversation)
	{
		await Clients.Caller.SendAsync("ReceiveConversation", conversation.Id, conversation.OfferId,
			conversation.OfferTitle, conversation.OfferPrice, conversation.OfferImageUrl, conversation.WithUser,
			conversation.LastMessage?.Date, conversation.LastMessage?.Text, conversation.IsRead);
	}

	private async Task NewMessage(int chat, int user, string userName, DateTimeOffset date, string text)
	{
		await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", chat, user, userName, date, text);
	}
	
	private async Task UpdateConversation(string connectionId, ConversationInfoDto conversation, DateTimeOffset date, string text, bool isRead)
	{
		await Clients.Client(connectionId).SendAsync("ReceiveConversation", conversation.Id, conversation.OfferId,
			conversation.OfferTitle, conversation.OfferPrice, conversation.OfferImageUrl, conversation.WithUser,
			date, text, isRead);
	}
	
	private async Task UpdateConversation(ConversationInfoDto conversation, DateTimeOffset date, string text, bool isRead)
	{
		await Clients.Caller.SendAsync("ReceiveConversation", conversation.Id, conversation.OfferId,
			conversation.OfferTitle, conversation.OfferPrice, conversation.OfferImageUrl, conversation.WithUser,
			date, text, isRead);
	}
}