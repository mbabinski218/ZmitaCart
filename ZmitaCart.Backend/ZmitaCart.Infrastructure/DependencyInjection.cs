using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Infrastructure.Persistence;
using ZmitaCart.Infrastructure.Repositories;

namespace ZmitaCart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TestDb")));
        
        services.AddScoped<IWeatherRepository, WeatherRepository>();

        return services;
    }
}