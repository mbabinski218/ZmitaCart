using AutoMapper;
using ZmitaCart.Application.Commands.OfferCommands;
using ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Queries.UserQueries.LoginUser;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterUserCommand, RegisterUserDto>();
        CreateMap<LoginUserQuery, LoginUserDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, SuperiorCategoryDto>();
        CreateMap<ExternalAuthenticationCommand, ExternalAuthDto>();
        CreateMap<CreateOfferCommand, CreateOfferDto>();
    }
}