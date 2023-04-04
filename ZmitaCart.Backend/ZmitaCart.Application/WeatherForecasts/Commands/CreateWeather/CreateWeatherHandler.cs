using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.WeatherForecasts.Commands.CreateWeather;

public class CreateWeatherHandler : IRequestHandler<CreateWeatherCommand, Weather>
{
    private readonly IWeatherRepository _weatherRepository;

    public CreateWeatherHandler(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }
    
    public async Task<Weather> Handle(CreateWeatherCommand request, CancellationToken cancellationToken)
    {
        if (request.Day == null)
            throw new Exception("Error");
        
        var weather = new Weather
        {
            Day = request.Day, 
            Temperature = request.Temperature
        };

        var created = await _weatherRepository.AddWeatherAsync(weather, cancellationToken);

        return created;
    }
}