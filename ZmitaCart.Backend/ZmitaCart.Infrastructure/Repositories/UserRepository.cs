using ZmitaCart.Application.Dtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;

	public UserRepository(IJwtTokenGenerator jwtTokenGenerator)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
	}

	public string Login(LoginUserDto user)
	{
		throw new NotImplementedException();
	}
}