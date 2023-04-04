using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WeatherRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Weather GetWeatherById(int id)
    {
        var weather = _dbContext.WeatherList.FirstOrDefault(w => w.Id == id);

        if (weather == null)
            throw new Exception("Error");

        return weather;
    }

    public async Task<Weather> AddWeatherAsync(Weather weather, CancellationToken cancellationToken)
    {
        var created = await _dbContext.WeatherList.AddAsync(weather, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return created.Entity;
    }
}