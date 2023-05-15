using ZmitaCart.Application.Services;
using System.Security.Claims;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.Id);
}
