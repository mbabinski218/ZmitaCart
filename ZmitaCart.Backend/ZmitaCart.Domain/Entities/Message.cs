using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Message : Entity<int>
{
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; set; } 
	public int UserId { get; set; }
	public virtual User User { get; set; } = null!;
	public int ConversationId { get; set; }
	public virtual Conversation Conversation { get; set; } = null!;
	
	public static Message Create(int userId, User user, int conversationId, Conversation conversation, string text)
	{
		var message = new Message
		{
			Text = text,
			UserId = userId,
			User = user,
			ConversationId = conversationId,
			Conversation = conversation,
			Date = DateTimeOffset.Now
		};

		return message;
	}
}