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

	public async Task Create(int offerId, string userId)
	{
		var createdChatEvent = new CreatedChat(userId, offerId);
		await _mediator.Publish(createdChatEvent);
		
		await Groups.AddToGroupAsync(Context.ConnectionId, createdChatEvent.ChatId.ToString());
	}
	
	public async Task Join(int chat, string userId)
	{ 
		await Groups.AddToGroupAsync(Context.ConnectionId, chat.ToString());
		await _mediator.Publish(new JoinedChat(chat, userId));
	}
	
	public async Task RestoreMessages(int userId, string user, DateTimeOffset date, string text)
	{
		var message = new Message
		{
			AuthorId = userId,
			AuthorName = user,
			Date = date,
			Content = text
		};
		
		await Clients.Caller.SendAsync("ReceiveMessage", message);
	}
	
	public async Task SendMessage(int chat, string userId, string userName, string text)
	{
		var message = new Message
		{
			AuthorId = int.Parse(userId),
			AuthorName = userName,
			Date = DateTimeOffset.Now,
			Content = text
		};
		
		await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", message);

		await _mediator.Publish(new MessageSent(userId, chat, text));
	}
}

internal class Message
{
	public int AuthorId { get; set; }
	public string AuthorName { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
	public string Content { get; set; } = null!;
}