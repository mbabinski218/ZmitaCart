using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Picture : AggregateRoot<int>
{
    public int OfferId { get; set; }
    public virtual Offer Offer { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
    public DateTimeOffset CreationTime { get; set; }
}