
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	public Task JoinAsync(string chat, CancellationToken cancellationToken);
	public Task RestoreMessagesAsync(int userId, string user, string chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
	public Task SendMessageAsync(string user, string chat, string text, DateTimeOffset date, CancellationToken cancellationToken);
}