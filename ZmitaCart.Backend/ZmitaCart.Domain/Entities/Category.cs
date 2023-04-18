using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Category : AggregateRoot<int>
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public string? IconName { get; set; }
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? Children { get; set; }
    public virtual ICollection<CategoryOffer>? CategoryOffers { get; set; }
}