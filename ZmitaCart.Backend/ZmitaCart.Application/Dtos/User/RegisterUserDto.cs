using System.ComponentModel.DataAnnotations;

namespace ZmitaCart.Application.Dtos.User;

public record RegisterUserDto
{
	[Required(ErrorMessage = "Field is required")]
	[EmailAddress(ErrorMessage = "Wrong format")]
	public string Email { get; init; } = null!;
	
	[Required(ErrorMessage = "Field is required")]
	public string FirstName { get; init; } = null!;	
	
	[Required(ErrorMessage = "Field is required")]
	public string LastName { get; init; } = null!;

	[Required(ErrorMessage = "Field is required")]
	[MinLength(5, ErrorMessage = "Password is too short")]
	public string Password { get; init; } = null!;

	[Required(ErrorMessage = "Field is required")]
	[Compare("Password", ErrorMessage = "Passwords are not the same")]
	public string ConfirmedPassword { get; init; } = null!;

	public int RoleId { get; } = 1;
}