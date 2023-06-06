using System.IdentityModel.Tokens.Jwt;
using ZmitaCart.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? UserId
	{
		get
		{
			var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
				.FirstOrDefault()
				?.Split(" ")
				.LastOrDefault();
			
			if (token is null)
				return null;

			var handler = new JwtSecurityTokenHandler();

			if (!handler.CanReadToken(token))
				return null;
			
			var jwtToken = handler.ReadJwtToken(token);
			var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimNames.Id)?.Value;
			
			return userId;
		}
	}
	public string? UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.Email);
	public string? UserFirstName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.FirstName);
	public string? UserLastName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.LastName);
	public string? UserRole => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimNames.Role);
}
