namespace ZmitaCart.Domain.Entities;

public class UserConversation
{
	public int ConversationId { get; set; }
	public virtual Conversation Conversation { get; set; } = null!;
	public int UserId { get; set; }
	public virtual User User { get; set; } = null!;
}