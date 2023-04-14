using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> categoryModelBuilder)
	{
		categoryModelBuilder.Property(c => c.Name).IsRequired();

		categoryModelBuilder
			.HasOne(c => c.Parent)
			.WithMany(c => c.Children)
			.HasForeignKey(c => c.ParentId);
	}
}