using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IJwtTokenGenerator
{
	public string CreateToken(UserClaimsDto userClaims);
}