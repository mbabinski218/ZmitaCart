using FluentResults;
using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IGoogleAuthentication
{
	public Task<Result<string>> AuthenticateAsync(ExternalAuthDto externalAuthDto);
}