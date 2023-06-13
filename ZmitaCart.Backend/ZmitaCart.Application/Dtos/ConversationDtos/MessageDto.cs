namespace ZmitaCart.Application.Dtos.ConversationDtos;

public class MessageDto
{
	public int ChatId { get; set; }
	public int UserId { get; set; }
	public string UserName { get; set; } = null!;
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
}