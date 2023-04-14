using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public Task RegisterAsync(RegisterUserDto registerUserDto);
	public Task<string> LoginAsync(LoginUserDto loginUserDto);
	public Task ChangeRoleAsync(string userEmail, Role newRole);
}