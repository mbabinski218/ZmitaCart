
namespace ZmitaCart.Application.Hubs;

public interface IChatHub
{
	Task JoinGroup(string chat, CancellationToken cancellationToken);
	Task SendMessage(string user, string chat, string text, DateTimeOffset time, CancellationToken cancellationToken);
}