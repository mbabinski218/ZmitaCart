namespace ZmitaCart.Application.Dtos.OfferDtos;

public class SearchOfferDto
{
	public string? Title { get; set; }
	public int? CategoryId { get; set; }
	public string? Condition { get; set; }
	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }
	public bool? PriceAscending { get; set; }
	public bool? CreatedAscending { get; set; }
}