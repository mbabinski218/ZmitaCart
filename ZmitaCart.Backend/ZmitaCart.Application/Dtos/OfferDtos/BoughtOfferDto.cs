namespace ZmitaCart.Application.Dtos.OfferDtos;

public class BoughtOfferDto
{
	public int Id { get; set; }
	public decimal Price { get; set; }
	public string Title { get; set; } = null!;
	public int Quantity { get; set; }
	public DateTimeOffset BoughtAt { get; set; }
	public decimal TotalPrice { get; set; }
}