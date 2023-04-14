using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class FeedbackConfiguration: IEntityTypeConfiguration<Feedback>
{
	public void Configure(EntityTypeBuilder<Feedback> feedbackModelBuilder)
	{
		feedbackModelBuilder.Property(f => f.RaterId).IsRequired();
		feedbackModelBuilder.Property(f => f.Rating).IsRequired();
	}
}