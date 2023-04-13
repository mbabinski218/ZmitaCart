using ZmitaCart.Application.Dtos.User;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public Task RegisterAsync(RegisterUserDto user);
	public Task<string> LoginAsync(LoginUserDto user);
	public Task ChangeRoleAsync(string userEmail, Role newRole);
}