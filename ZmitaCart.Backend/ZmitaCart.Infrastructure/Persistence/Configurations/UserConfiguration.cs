using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> userModelBuilder)
	{
		userModelBuilder.Property(u => u.Email).IsRequired();
		userModelBuilder.Property(u => u.FirstName).IsRequired();
		userModelBuilder.Property(u => u.LastName).IsRequired();
		userModelBuilder.Property(u => u.PasswordHash).IsRequired();
		userModelBuilder.Property(u => u.PasswordSalt).IsRequired();
		
		userModelBuilder.OwnsOne(u => u.Address);
		userModelBuilder.OwnsOne(u => u.Role);
		
		
	}
}