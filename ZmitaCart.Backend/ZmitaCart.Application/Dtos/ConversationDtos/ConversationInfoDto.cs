namespace ZmitaCart.Application.Dtos.ConversationDtos;

public class ConversationInfoDto
{
	public int ConversationId { get; set; }
	public string WithUser { get; set; } = null!;
	public string? LastMessage { get; set; } = null!;
	public DateTimeOffset? LastMessageCreatedAt { get; set; }
	public int OfferId { get; set; }
	public string OfferTitle { get; set; } = null!;
	public string? OfferImageUrl { get; set; }
	public string OfferPrice { get; set; } = null!;
}