using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.API.Hubs;

public class ChatHub : Hub
{
	private readonly IPublisher _mediator;
	private readonly IConversationRepository _conversationRepository;

	public ChatHub(IPublisher mediator, IConversationRepository conversationRepository)
	{
		_mediator = mediator;
		_conversationRepository = conversationRepository;
	}

	public async Task Join(int chat, string userId)
	{ 
		await Groups.AddToGroupAsync(Context.ConnectionId, chat.ToString());

		var messages = await _conversationRepository.GetMessagesAsync(chat);
		
		foreach (var message in messages)
		{
			await RestoreMessages(message.UserId, message.UserName, message.Date, message.Text);
		}
	}
	
	public async Task RestoreMessages(int userId, string user, DateTimeOffset date, string text)
	{
		await Clients.Caller.SendAsync(
			"ReceiveMessage", userId, user, date, text);
	}
	
	public async Task SendMessage(int chat, string userId, string userName, string text)
	{
		await Clients.Group(chat.ToString()).SendAsync(
			"ReceiveMessage", int.Parse(userId), userName, DateTimeOffset.Now, text);

		await _mediator.Publish(new MessageSent(userId, chat, text));
	}
}