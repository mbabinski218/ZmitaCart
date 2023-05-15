using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Conversation : AggregateRoot<int>
{
	public List<Message> Messages { get; set; } = new();
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; } = null!;
	public List<UserConversation> UserConversations { get; set; } = new();
}   