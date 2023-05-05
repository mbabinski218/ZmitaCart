using Mapster;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class ConversationMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Message, MessageDto>()
			.Map(dest => dest.UserName, src => src.User.UserName);

		config.ForType<UserConversation, ConversationInfoDto>()
			.Map(dest => dest.OfferId, src => src.Conversation.OfferId)
			.Map(dest => dest.OfferTitle, src => src.Conversation.Offer.Title)
			.Map(dest => dest.OfferPrice, src => src.Conversation.Offer.Price)
			.Map(dest => dest.OfferImageUrl, src => src.Conversation.Offer.Pictures == null || !src.Conversation.Offer.Pictures.Any()
				? null
				: Path.Combine(Path.GetFullPath("wwwroot"), src.Conversation.Offer.Pictures.OrderBy(p => p.CreationTime).First().Name))
			.Map(dest => dest.LastMessage, src => src.Conversation.Messages == null
				? null
				: src.Conversation.Messages.OrderByDescending(m => m.Date).First().Text)
			.Map(dest => dest.LastMessageCreatedAt, src => src.Conversation.Messages == null
				? (DateTimeOffset?)null
				: src.Conversation.Messages.OrderByDescending(m => m.Date).First().Date);
	}
}