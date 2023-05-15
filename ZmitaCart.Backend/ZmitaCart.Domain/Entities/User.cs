using Microsoft.AspNetCore.Identity;
using ZmitaCart.Domain.ValueObjects;
namespace ZmitaCart.Domain.Entities;

public class User : IdentityUser<int>
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Address Address { get; set; } = null!;
	public virtual List<Bought> Bought { get; set; } = new();
	public virtual List<UserConversation> Chats { get; set; } = new();
	public virtual List<UserOffer> Favorites { get; set; } = new();
	public virtual List<Feedback> Feedbacks { get; set; } = new();
	public virtual List<Offer> Offers { get; set; } = new();
}