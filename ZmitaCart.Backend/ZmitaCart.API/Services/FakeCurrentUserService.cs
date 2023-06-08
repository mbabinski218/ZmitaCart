using System.IdentityModel.Tokens.Jwt;
using ZmitaCart.Application.Services;

namespace ZmitaCart.API.Services;

public class FakeCurrentUserService : ICurrentUserService
{
	public JwtSecurityToken? UserToken => new();
	public string? UserId => "1";
	public string? UserRole => "Admin";
}