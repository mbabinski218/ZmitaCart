using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Domain.Enums;

namespace ZmitaCart.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    
    public DbSet<Weather> WeatherList { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;
    public DbSet<UserOffer> Favorites { get; set; } = null!;
    public DbSet<Bought> Bought { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Conversation> Conversations { get; set; } = null!;
    public DbSet<UserConversation> Chats { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}