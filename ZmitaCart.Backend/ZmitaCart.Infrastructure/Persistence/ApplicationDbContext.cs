using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }
    
    public DbSet<Weather> WeatherList { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}