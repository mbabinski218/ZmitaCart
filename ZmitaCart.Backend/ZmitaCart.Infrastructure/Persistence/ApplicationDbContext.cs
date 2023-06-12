using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.Interceptors;

namespace ZmitaCart.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
        
        // Database.EnsureDeleted();
        // Database.EnsureCreated();
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;
    public DbSet<UserOffer> Favorites { get; set; } = null!;
    public DbSet<Bought> Bought { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Conversation> Conversations { get; set; } = null!;
    public DbSet<UserConversation> Chats { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Picture> Pictures { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}