using System.IdentityModel.Tokens.Jwt;

namespace ZmitaCart.Application.Services;

public interface ICurrentUserService
{
	public JwtSecurityToken? UserToken { get; }
	public string? UserId { get; }
	public string? UserRole { get; }
}