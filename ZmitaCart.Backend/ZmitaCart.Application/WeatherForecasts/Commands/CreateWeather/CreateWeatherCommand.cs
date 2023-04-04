using MediatR;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.WeatherForecasts.Commands.CreateWeather;

public record CreateWeatherCommand : IRequest<Weather>
{
    public string? Day { get; set; }
    public int Temperature { get; set; } 
}