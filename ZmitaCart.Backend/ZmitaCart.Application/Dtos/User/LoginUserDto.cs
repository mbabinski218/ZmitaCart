using System.ComponentModel.DataAnnotations;

namespace ZmitaCart.Application.Dtos.User;

public record LoginUserDto
{
	[Required(ErrorMessage = "Field is required")]
	[EmailAddress(ErrorMessage = "Wrong format")]
	public string Email { get; init; } = null!;

	[Required(ErrorMessage = "Field is required")]
	public string Password { get; init; } = null!;
}