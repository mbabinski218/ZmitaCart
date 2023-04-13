using ZmitaCart.Application.Dtos;

namespace ZmitaCart.Application.Interfaces;

public interface IJwtTokenGenerator
{
	public string CreateToken(int userId, string userName, string lastName, string email, string role);
}