using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Queries.WeatherForecast.GetWeather;

public class GetWeatherHandler : IRequestHandler<GetWeatherQuery, Weather>
{
    private readonly IWeatherRepository _weatherRepository;

    public GetWeatherHandler(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }
    
    public async Task<Weather> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        var result = _weatherRepository.GetWeatherById(request.Id);
        return await Task.FromResult(result);
    }
}