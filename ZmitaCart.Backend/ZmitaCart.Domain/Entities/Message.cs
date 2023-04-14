using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Message : Entity<int>
{
	public string Text { get; set; } = null!;
	public DateTimeOffset Date { get; set; } 
	public int UserId { get; set; }
	public virtual User User { get; set; }
	public int ChatId { get; set; }
	public virtual Chat Chat { get; set; }
}