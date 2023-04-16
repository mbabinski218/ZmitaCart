using System.Security.Claims;

namespace ZmitaCart.Application.Interfaces;

public interface IJwtTokenGenerator
{
	public string CreateToken(IEnumerable<Claim> userClaims);
}