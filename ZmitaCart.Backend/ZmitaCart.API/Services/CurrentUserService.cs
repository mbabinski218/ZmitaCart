using System.IdentityModel.Tokens.Jwt;
using ZmitaCart.Application.Services;
using ZmitaCart.API.Common;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public JwtSecurityToken? UserToken => Reader.ReadToken(_httpContextAccessor.HttpContext);
	public string? UserId => UserToken?.FindOrDefault(ClaimNames.Id);
	public string? UserRole => UserToken?.FindOrDefault(ClaimNames.Role);
}