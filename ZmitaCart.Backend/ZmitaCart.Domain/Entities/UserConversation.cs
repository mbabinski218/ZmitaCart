using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class UserConversation : Entity<int>
{
	public int ConversationId { get; set; }
	public virtual Conversation Conversation { get; set; } = null!;
	public int UserId { get; set; }
	public virtual User User { get; set; } = null!;
	
}