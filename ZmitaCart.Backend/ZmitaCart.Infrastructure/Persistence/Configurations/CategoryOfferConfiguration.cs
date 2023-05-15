using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class CategoryOfferConfiguration : IEntityTypeConfiguration<CategoryOffer>
{
    public void Configure(EntityTypeBuilder<CategoryOffer> builder)
    {
        builder.Property(co => co.OfferId).IsRequired();
        builder.Property(co => co.CategoryId).IsRequired();

        builder
            .HasKey(co => new { co.OfferId, co.CategoryId });

        builder
            .HasOne(co => co.Offer)
            .WithMany(o => o.CategoryOffers)
            .HasForeignKey(co => co.OfferId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(co => co.Category)
            .WithMany(c => c.CategoryOffers)
            .HasForeignKey(co => co.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}