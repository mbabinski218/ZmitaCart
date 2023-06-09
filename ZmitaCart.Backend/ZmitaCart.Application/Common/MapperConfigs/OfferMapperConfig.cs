using Mapster;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class OfferMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Offer, OfferInfoDto>()
			.Map(dest => dest.City, src => src.User.Address.City)
			.Map(dest => dest.ImageName, src => !src.Pictures.Any()
				? null
				: src.Pictures.OrderBy(p => p.CreationTime).First().Name)
			.Map(dest => dest.AuthorEmail, src => src.User.Email)
			.Map(dest => dest.AuthorName, src => src.User.FirstName);

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