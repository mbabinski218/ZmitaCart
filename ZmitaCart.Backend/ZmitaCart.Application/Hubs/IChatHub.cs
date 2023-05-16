
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	public Task JoinAsync(int chat, CancellationToken cancellationToken);
	public Task RestoreMessagesAsync(int userId, string user, int chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
	public Task SendMessageAsync(string user, int chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
}