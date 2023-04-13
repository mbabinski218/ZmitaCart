namespace ZmitaCart.Application.Dtos.User;

public record UserDto
{
	public string Email { get; init; } = null!;
	public string FirstName { get; init; } = null!;
	public string LastName { get; init; } = null!;
	public string? PhoneNumber { get; init; }
}