using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Events;

namespace ZmitaCart.API.Hubs;

[Authorize(Roles = Role.user + "," + Role.administrator)]
public class ChatHub : Hub, IChatHub
{
	private readonly IPublisher _mediator;

	public ChatHub(IPublisher mediator)
	{
		_mediator = mediator;
	}

	public async Task Join(int chat, string userId)
	{ 
		await Groups.AddToGroupAsync(Context.ConnectionId, chat.ToString());
		await _mediator.Publish(new JoinedChat(chat, userId));
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