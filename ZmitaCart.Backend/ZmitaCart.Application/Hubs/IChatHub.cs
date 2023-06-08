
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	public Task Join(int chat, CancellationToken cancellationToken);
	public Task RestoreMessages(int userId, string user, int chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
	public Task SendMessage(string user, int chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
}