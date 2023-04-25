using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Dtos.OfferDtos;

public class OfferInfoDto
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public Address Address { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; } = null!;
}