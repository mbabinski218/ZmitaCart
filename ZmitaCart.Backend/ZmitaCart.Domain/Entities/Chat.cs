using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Chat : AggregateRoot<int>
{
	public ICollection<Message>? Messages { get; set; }
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; }
	public int SellerId { get; set; }
	public virtual User Seller { get; set; }
	public int BuyerId { get; set; }
	public virtual User Buyer { get; set; }
}   