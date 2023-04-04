using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Interfaces;

public interface IWeatherRepository
{
    Weather GetWeatherById(int id);
    Task<Weather> AddWeatherAsync(Weather weather, CancellationToken cancellationToken);
}