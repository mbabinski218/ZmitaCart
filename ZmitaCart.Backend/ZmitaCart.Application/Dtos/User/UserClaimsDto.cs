namespace ZmitaCart.Application.Dtos.User;

public record UserClaimsDto
{
	public int Id { get; init; }
	public string Email { get; init; } = null!;
	public string FirstName { get; init; } = null!;
	public string Role { get; init; } = null!;
}