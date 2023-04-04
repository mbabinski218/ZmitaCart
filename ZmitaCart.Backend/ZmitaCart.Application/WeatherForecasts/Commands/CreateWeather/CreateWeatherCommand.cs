using MediatR;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.WeatherForecasts.Commands.CreateWeather;

public record CreateWeatherCommand : IRequest<Weather>
{
    public string? Day { get; init; }
    public int Temperature { get; init; } 
}