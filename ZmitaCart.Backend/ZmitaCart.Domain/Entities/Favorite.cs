using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Favorite : Entity<int>
{
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; }
	public int UserId { get; set; }
	public virtual User User { get; set; }
}