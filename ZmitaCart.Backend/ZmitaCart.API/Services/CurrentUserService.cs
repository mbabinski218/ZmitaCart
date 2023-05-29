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
	public string? UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.Email);
	public string? UserFirstName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.FirstName);
	public string? UserLastName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.LastName);
	public string? UserRole => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.Role);
}
