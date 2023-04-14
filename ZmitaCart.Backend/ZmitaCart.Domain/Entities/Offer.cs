using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Enums;

namespace ZmitaCart.Domain.Entities;

public class Offer : AggregateRoot<int>
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public bool IsAvailable { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
	public Condition Condition { get; set; }
	public int CategoryId { get; set; }
	public Category Category { get; set; } = null!;
	public int UserId { get; set; }
	public User User { get; set; } = null!;
	public ICollection<Favorite> Favorites { get; set; }
	public ICollection<Bought> Bought { get; set; }
	// public ICollection<OfferImage>? OfferImages { get; set; }
}