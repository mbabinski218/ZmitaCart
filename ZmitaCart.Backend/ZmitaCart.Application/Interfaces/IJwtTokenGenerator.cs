using ZmitaCart.Application.Dtos.User;

namespace ZmitaCart.Application.Interfaces;

public interface IJwtTokenGenerator
{
	public string CreateToken(UserClaimsDto userClaims);
}