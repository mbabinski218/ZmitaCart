using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IGoogleAuthentication
{
	public Task<string> AuthenticateAsync(ExternalAuthDto externalAuthDto);
}