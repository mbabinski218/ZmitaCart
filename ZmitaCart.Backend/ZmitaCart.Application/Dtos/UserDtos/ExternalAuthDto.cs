namespace ZmitaCart.Application.Dtos.UserDtos;

public record ExternalAuthDto
{
	    public string Provider { get; set; } = null!;
	    public string Token { get; set; } = null!;
}