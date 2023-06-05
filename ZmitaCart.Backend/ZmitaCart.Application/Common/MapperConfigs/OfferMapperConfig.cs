using Mapster;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class OfferMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Offer, OfferInfoDto>()
			.Map(dest => dest.Address, src => src.User.Address)
			.Map(dest => dest.ImageName, src => !src.Pictures.Any()
				? null
				: src.Pictures.OrderBy(p => p.CreationTime).First().Name);

		config.ForType<Offer, OfferInfoWithCategoryNameDto>()
			.Map(dest => dest.CategoryName, src => src.Category.Name)
			.Map(dest => dest.Address, src => src.User.Address)
			.Map(dest => dest.ImageName, src => !src.Pictures.Any()
				? null
				: src.Pictures.OrderBy(p => p.CreationTime).First().Name);

		config.ForType<Offer, OfferDto>()
			.Map(dest => dest.Address, src => src.User.Address)
			.Map(dest => dest.PicturesNames, src => !src.Pictures.Any()
				? null
				: src.Pictures.Select(p => p.Name));

		config.ForType<Bought, BoughtOfferDto>()
			.Map(dest => dest.Id, src => src.Offer.Id)
			.Map(dest => dest.Price, src => src.Offer.Price)
			.Map(dest => dest.Title, src => src.Offer.Title);
	}
}