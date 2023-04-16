using AutoMapper;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Queries.UserQueries.LoginUser;

namespace ZmitaCart.Application.Common;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<RegisterUserCommand, RegisterUserDto>();
		CreateMap<LoginUserQuery, LoginUserDto>();
	}
}