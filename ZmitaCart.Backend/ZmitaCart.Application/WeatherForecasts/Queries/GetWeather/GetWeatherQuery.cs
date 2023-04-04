using MediatR;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.WeatherForecasts.Queries.GetWeather;

public class GetWeatherQuery : IRequest<Weather>
{
    public int Id { get; set; }
}