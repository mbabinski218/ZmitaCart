using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class UserOffer : Entity<int>
{
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; } = null!;
	public int UserId { get; set; }
	public virtual User User { get; set; } = null!;
}