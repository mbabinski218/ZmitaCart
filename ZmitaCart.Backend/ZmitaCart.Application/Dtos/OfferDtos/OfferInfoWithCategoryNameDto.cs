using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Dtos.OfferDtos;

public record OfferInfoWithCategoryNameDto
{
	public int Id { get; set; }
	public string CategoryName { get; set; } = null!;
	public string Title { get; set; } = null!;
	public decimal Price { get; set; }
	public Address Address { get; set; } = null!;
	public string Condition { get; set; } = null!;
	public int Quantity { get; set; }
	public string? ImageName { get; set; }
	public bool IsFavourite { get; set; } = false;
}