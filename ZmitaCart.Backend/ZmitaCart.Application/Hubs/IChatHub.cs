
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	public Task Join(int chat, string userId);
	public Task RestoreMessages(int userId, string user, DateTimeOffset date, string text);
	public Task SendMessage(int chat, string userId, string userName, string text);
}