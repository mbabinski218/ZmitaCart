namespace ZmitaCart.Application.Dtos.ConversationDtos;

public class MessageDto
{
	public int UserId { get; set; }
	public string User { get; set; } = null!;
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
}