using Microsoft.AspNetCore.Identity;
using ZmitaCart.Domain.ValueObjects;
namespace ZmitaCart.Domain.Entities;

public class User : IdentityUser<int>
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Address Address { get; set; } = null!;
	public ICollection<Bought>? Bought { get; set; }
	public ICollection<UserConversation>? Chats { get; set; }
	public ICollection<UserOffer>? Favorites { get; set; }
	public ICollection<Feedback>? Feedbacks { get; set; }
	public ICollection<Offer>? Offers { get; set; }
}