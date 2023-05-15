namespace ZmitaCart.Application.Dtos.UserDtos;

public record LoginUserDto
{
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
}