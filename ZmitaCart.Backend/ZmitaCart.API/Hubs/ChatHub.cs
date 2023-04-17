using Microsoft.AspNetCore.SignalR;

namespace ZmitaCart.API.Hubs;

public class ChatHub : Hub
{
	// public readonly IChatService _chatService;
	//
	// public ChatHub(IChatService chatService)
	// {
	// 	_chatService = chatService;
	// }

	public async Task JoinGroup(int chatId, CancellationToken cancellationToken)
	{
		var chatName = "getChatName";
		await Groups.AddToGroupAsync(Context.ConnectionId, chatName, cancellationToken);
	}
	
	public async Task SendMessage(int userId, int chatId, string content, CancellationToken cancellationToken)
	{
		var chatName = "getChatName";
		var authorName = "getUserName";
		var date = DateTime.Now.ToString("HH:mm");
		
		await Clients.Group(chatName).SendAsync("ReceiveMessage", authorName, date, content, cancellationToken);
	}
}