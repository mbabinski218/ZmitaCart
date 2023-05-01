using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class UserConversationConfiguration : IEntityTypeConfiguration<UserConversation>
{
	public void Configure(EntityTypeBuilder<UserConversation> userConversationModelBuilder)
	{
		userConversationModelBuilder
			.HasKey(uc => new { uc.ConversationId, uc.UserId });
		
		userConversationModelBuilder
			.HasOne(uc => uc.Conversation)
			.WithMany(o => o.Users)
			.HasForeignKey(uc => uc.ConversationId)
			.OnDelete(DeleteBehavior.NoAction);
		
		userConversationModelBuilder
			.HasOne(uc => uc.User)
			.WithMany(u => u.Chats)
			.HasForeignKey(uc => uc.UserId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}