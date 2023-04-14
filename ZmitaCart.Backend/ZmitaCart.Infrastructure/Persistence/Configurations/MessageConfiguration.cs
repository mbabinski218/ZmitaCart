using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class MessageConfiguration: IEntityTypeConfiguration<Message>
{
	public void Configure(EntityTypeBuilder<Message> messageModelBuilder)
	{
		messageModelBuilder.Property(m => m.Text).IsRequired();
		messageModelBuilder.Property(m => m.Date).IsRequired();
		messageModelBuilder.Property(m => m.UserId).IsRequired();
		messageModelBuilder.Property(m => m.ChatId).IsRequired();
	}
}