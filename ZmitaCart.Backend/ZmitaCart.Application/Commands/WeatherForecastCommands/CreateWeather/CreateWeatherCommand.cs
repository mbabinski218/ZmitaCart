using MediatR;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Commands.WeatherForecastCommands.CreateWeather;

public record CreateWeatherCommand : IRequest<Weather>
{
    public string? Day { get; init; }
    public int Temperature { get; init; } 
}