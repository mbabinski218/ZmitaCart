using ZmitaCart.Application.Dtos;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public string Login(LoginUserDto user);
}