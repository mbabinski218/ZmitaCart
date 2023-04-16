using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Dtos.UserDtos;

public record RegisterUserDto
{
	public string Email { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Password { get; set; } = null!;
	public Role? Role { get; set; }
}