using Mapster;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class ConversationMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Message, MessageDto>()
			.Map(dest => dest.ChatId, src => src.ConversationId)
			.Map(dest => dest.UserName, src => src.User.FirstName);
			
		
		config.ForType<UserConversation, ConversationInfoDto>()
			.Map(dest => dest.OfferId, src => src.Conversation.OfferId)
			.Map(dest => dest.OfferTitle, src => src.Conversation.Offer.Title)
			.Map(dest => dest.OfferPrice, src => src.Conversation.Offer.Price)
			.Map(dest => dest.OfferImageUrl, src => src.Conversation.Offer.Pictures.Any()
				? src.Conversation.Offer.Pictures.OrderBy(p => p.CreationTime).First().Name
				: null)
			.Map(dest => dest.WithUser, src => GetUserInfo(src));

		config.ForType<Conversation, ConversationInfoDto>()
			.Map(dest => dest.OfferId, src => src.OfferId)
			.Map(dest => dest.OfferTitle, src => src.Offer.Title)
			.Map(dest => dest.OfferPrice, src => src.Offer.Price)
			.Map(dest => dest.OfferImageUrl, src => src.Offer.Pictures.Any()
				? src.Offer.Pictures.OrderBy(p => p.CreationTime).First().Name
				: null);
			//.Map(dest => dest.WithUser, src => GetUserInfo(src));

		config.ForType<Conversation, ConversationDto>()
			.Map(dest => dest.OfferId, src => src.OfferId)
			.Map(dest => dest.OfferTitle, src => src.Offer.Title)
			.Map(dest => dest.OfferPrice, src => src.Offer.Price)
			.Map(dest => dest.OfferImageUrl, src => src.Offer.Pictures.Any()
				? src.Offer.Pictures.OrderBy(p => p.CreationTime).First().Name
				: null)
			.Map(dest => dest.LastMessage, src => src.Messages.Any()
				? src.Messages.OrderByDescending(m => m.Date).First().Adapt<MessageDto>()
				: null);
		//.Map(dest => dest.WithUser, src => GetUserInfo(src));
	}
	
	private string GetUserInfo(UserConversation src)
	{
		var user = src.Conversation.UserConversations.First(uc => uc.UserId != src.UserId).User;
		return $"{user.FirstName} {user.LastName}";
	}
}