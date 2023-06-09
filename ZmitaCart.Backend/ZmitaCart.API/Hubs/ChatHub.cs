using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZmitaCart.Application.Events;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Hubs;

[Authorize(Roles = Role.user + "," + Role.administrator)]
public class ChatHub : Hub, IChatHub
{
	private readonly IPublisher _mediator;

	public ChatHub(IPublisher mediator)
	{
		_mediator = mediator;
	}

	public async Task Join(int chat)
	{
		try
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, chat.ToString());
			//await Connect();
			//await _mediator.Publish(new JoinedChat(chat));
		}
		catch
		{
			await Disconnect();
			throw;
		}
	}
	
	public async Task RestoreMessages(int userId, string user, int chat, string text)
	{
		await Clients.Caller.SendAsync("ReceiveMessage", new
		{
			UserId = userId,
			UserName = user, 
			Date = DateTimeOffset.Now, 
			Content = text
		});
	}
	
	public async Task SendMessage(string user, int chat, string text)
	{
		await Clients.Group(chat.ToString()).SendAsync("ReceiveMessage", new
		{
			UserName = user, 
			Date = DateTimeOffset.Now, 
			Content = text
		});
		
		await _mediator.Publish(new MessageSent(user, chat, text));
	}
	
	private async Task Connect() => await Clients.Client(Context.ConnectionId).SendAsync("Connected");

	private async Task Disconnect() => await Clients.Client(Context.ConnectionId).SendAsync("Disconnected");
	
}
