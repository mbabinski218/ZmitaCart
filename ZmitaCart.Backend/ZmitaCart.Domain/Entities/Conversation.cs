using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Conversation : AggregateRoot<int>
{
	public ICollection<Message>? Messages { get; set; }
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; } = null!;
	public ICollection<UserConversation> UserConversations { get; set; } = null!;
}   