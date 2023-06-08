
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	public Task Join(int chat);
	public Task RestoreMessages(int userId, string user, int chat, string text);
	public Task SendMessage(string user, int chat, string text);
}