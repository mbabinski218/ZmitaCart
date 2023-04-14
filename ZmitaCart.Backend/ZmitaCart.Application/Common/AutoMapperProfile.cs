using AutoMapper;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;
using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Common;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<RegisterUserCommand, RegisterUserDto>();
	}
}