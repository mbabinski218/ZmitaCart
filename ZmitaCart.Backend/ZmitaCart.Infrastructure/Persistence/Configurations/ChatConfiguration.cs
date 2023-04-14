using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class ChatConfiguration: IEntityTypeConfiguration<Conversation>
{
	public void Configure(EntityTypeBuilder<Conversation> chatModelBuilder)
	{
		chatModelBuilder.Property(c => c.OfferId).IsRequired();
	}
}