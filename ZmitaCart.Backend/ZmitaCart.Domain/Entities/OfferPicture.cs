using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class OfferPicture : Entity<int>
{
    public int OfferId { get; set; }
    public virtual Offer Offer { get; set; } = null!;
    public string PictureName { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
}