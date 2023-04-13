using System.ComponentModel.DataAnnotations;

namespace ZmitaCart.Application.Dtos;

public record LoginUserDto
{
	[EmailAddress(ErrorMessage ="Wrong format")]
	public string? Email { get; init; }

	public string? Password { get; init; }
}