namespace ZmitaCart.Application.Dtos.ConversationDtos;

public class ConversationInfoDto
{
	public int Id { get; set; }
	public string WithUser { get; set; } = null!;
	public int OfferId { get; set; }
	public string OfferTitle { get; set; } = null!;
	public string? OfferImageUrl { get; set; }
	public decimal OfferPrice { get; set; }
	public bool IsRead { get; set; }
}