using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> userModelBuilder)
	{
		userModelBuilder.Property(u => u.FirstName).IsRequired();
		userModelBuilder.Property(u => u.LastName).IsRequired();

		userModelBuilder.OwnsOne(u => u.Address);
	}
}