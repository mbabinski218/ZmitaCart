using Microsoft.AspNetCore.Identity;
using ZmitaCart.Domain.ValueObjects;
namespace ZmitaCart.Domain.Entities;

public class User : IdentityUser<int>
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Address Address { get; set; } = null!;
	public virtual ICollection<Bought>? Bought { get; set; }
	public virtual ICollection<UserConversation>? Chats { get; set; }
	public virtual ICollection<UserOffer>? Favorites { get; set; }
	public virtual ICollection<Feedback>? Feedbacks { get; set; }
	public virtual ICollection<Offer>? Offers { get; set; }
}