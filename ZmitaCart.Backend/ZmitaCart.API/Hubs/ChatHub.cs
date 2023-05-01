using Microsoft.AspNetCore.SignalR;
using ZmitaCart.Application.Hubs;

namespace ZmitaCart.API.Hubs;

public class ChatHub : Hub, IChatHub
{
	public async Task JoinGroup(string chat, CancellationToken cancellationToken)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, chat, cancellationToken);
	}
	
	public async Task SendMessage(string user, string chat, string text, DateTimeOffset time, CancellationToken cancellationToken)
	{
		await Clients.Group(chat).SendAsync("ReceiveMessage", user, time, text, cancellationToken);
	}
}