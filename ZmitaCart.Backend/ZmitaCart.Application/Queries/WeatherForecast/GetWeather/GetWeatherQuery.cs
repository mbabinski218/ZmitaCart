using MediatR;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Queries.WeatherForecast.GetWeather;

public class GetWeatherQuery : IRequest<Weather>
{
    public int Id { get; init; }
}