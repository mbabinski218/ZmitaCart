using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> offerModelBuilder)
    {
        offerModelBuilder.Property(o => o.Title).IsRequired();
        offerModelBuilder.Property(o => o.Description).IsRequired();
        offerModelBuilder.Property(o => o.Condition).IsRequired();
        offerModelBuilder.Property(o => o.Quantity).IsRequired();
        offerModelBuilder.Property(o => o.IsAvailable).IsRequired();
        offerModelBuilder.Property(o => o.CreatedAt).IsRequired();
        offerModelBuilder.Property(o => o.Price).IsRequired().HasPrecision(8, 2);
        offerModelBuilder.HasOne(o => o.User)
            .WithMany(u => u.Offers)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}