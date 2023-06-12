using MediatR;
using Microsoft.AspNetCore.SignalR;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllMessages;
using ZmitaCart.Application.Queries.ConversationQueries.GetUserConversations;
using ZmitaCart.Application.Queries.ConversationQueries.ReadNotificationsStatus;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.API.Hubs;

public class ChatHub : Hub
{
	private readonly IMediator _mediator;
	private bool _isConnected;

	public ChatHub(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Join(string user)
	{
		var userId = int.Parse(user);
		var conversations = await _mediator.Send(new GetUserConversationsQuery(userId));

		if (conversations.IsFailed)
			return;

		foreach (var conversation in conversations.Value)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, conversation.ToString());
		}

		await ReadNotificationStatus(userId);
	}

	public async Task RestoreConversations(string user)
	{
		var userId = int.Parse(user);
		var conversations =  await _mediator.Send(new GetAllConversationsQuery(userId));
		
		if (conversations.IsFailed)
			return;

		foreach (var conversation in conversations.Value)
		{
			await RestoreConversation(conversation);
		}
		
		_isConnected = true;
	}
	
	public async Task RestoreMessages(int chat, string user)
	{
		var userId = int.Parse(user);
		var messages = await _mediator.Send(new GetAllMessagesQuery(chat, userId));

		if (messages.IsFailed)
			return;
		
		foreach (var message in messages.Value)
		{
			await RestoreMessage(message.UserId, message.UserName, message.Date, message.Text);
		}
		
		await ReadNotificationStatus(userId);
	}

	public async Task SendMessage(int chat, string user, string userName, string text)
	{
		var userId = int.Parse(user);
		var date = DateTimeOffset.Now;
		
		await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", userId, userName, date, text);
		await _mediator.Publish(new MessageSent(user, chat, date, text, _isConnected));

		if (!_isConnected)
		{
			await ReadNotificationStatus(userId);
		}
	}
	
	private async Task ReadNotificationStatus(int userId)
	{
		var status = await _mediator.Send(new ReadNotificationStatusQuery(userId));
		await Clients.Caller.SendAsync("NotificationsStatus", status);
	}

	private async Task RestoreMessage(int userId, string userName, DateTimeOffset date, string text)
	{
		await Clients.Caller.SendAsync("ReceiveMessage", userId, userName, date, text);
	}
	
	private async Task RestoreConversation(ConversationInfoDto conversation)
	{
		await Clients.Caller.SendAsync("ReceiveMessage", conversation);
	}
}