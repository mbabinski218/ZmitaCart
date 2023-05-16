using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class BoughtConfiguration : IEntityTypeConfiguration<Bought>
{
	public void Configure(EntityTypeBuilder<Bought> boughtModelBuilder)
	{
		boughtModelBuilder.Property(b => b.Quantity).IsRequired();
		boughtModelBuilder.Property(b => b.BoughtAt).IsRequired();
		boughtModelBuilder.Property(b => b.UserId).IsRequired();
		boughtModelBuilder.Property(b => b.OfferId).IsRequired();
		boughtModelBuilder.Property(o => o.TotalPrice).IsRequired().HasPrecision(8, 2);
	}
}