using AutoMapper;
using ZmitaCart.Application.Commands.OfferCommands.CreateOffer;
using ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;
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
        CreateMap<CreateOfferDto, Offer>();
        CreateMap<UpdateOfferCommand, UpdateOfferDto>();
        CreateMap<User, UserDto>();

        CreateMap<Offer, OfferInfoDto>()
            .ForMember(dto => dto.Address, op => op.MapFrom(
                o => o.User.Address))
            .ForMember(dto => dto.ImageUrl, op => op.MapFrom(
                o => o.Pictures == null || !o.Pictures.Any()
                    ? null
                    : Path.Combine(Path.GetFullPath("wwwroot"), o.Pictures.OrderBy(p => p.CreationTime).First().Name)));

        CreateMap<Offer, OfferDto>()
            .ForMember(dto => dto.Address, op => op.MapFrom(
                o => o.User.Address))
            .ForMember(dto => dto.PicturesUrls, op => op.MapFrom(
                o => o.Pictures == null || o.Pictures.Count == 0
                    ? null
                    : o.Pictures.Select(p => Path.Combine(Path.GetFullPath("wwwroot"), p.Name))));

        CreateMap<Bought, BoughtOfferDto>()
            .ForMember(dto => dto.Id, op => op.MapFrom(
                b => b.Offer.Id))
            .ForMember(dto => dto.Price, op => op.MapFrom(
                b => b.Offer.Price))
            .ForMember(dto => dto.Title, op => op.MapFrom(
                b => b.Offer.Title));
    }
}

