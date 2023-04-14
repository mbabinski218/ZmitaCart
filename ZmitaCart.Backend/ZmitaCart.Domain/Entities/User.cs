using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Domain.Entities;

public class User : AggregateRoot<int>
{
	public string Email { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Role Role { get; set; } = null!;
	public byte[] PasswordHash { get; set; } = null!;
	public byte[] PasswordSalt { get; set; } = null!;
	public string? Phone { get; set; }
	public Address? Address { get; set; }
	public ICollection<Bought>? Bought { get; set; }
	public ICollection<Chat>? Chats { get; set; }
	public ICollection<Favorite>? Favorites { get; set; }
	public ICollection<Feedback>? Feedbacks { get; set; }
	public ICollection<Offer>? Offers { get; set; }
}