using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Offer : AggregateRoot<int>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Condition { get; set; } = null!;
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<UserOffer>? Favorites { get; set; }
    public virtual ICollection<Bought>? Bought { get; set; }
    public virtual ICollection<CategoryOffer>? CategoryOffers { get; set; } //TODO remove
    public virtual ICollection<Picture>? Pictures { get; set; } //TODO change to Pictures
}