using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.Application.WeatherForecasts.Commands.CreateWeather;
using ZmitaCart.Application.WeatherForecasts.Queries.GetWeather;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Weather>> Create([FromBody] CreateWeatherCommand command)
    {
        var created = await _mediator.Send(command);
        return Created("/WeatherForecast/" + created.Id, created);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Weather>> Get([FromRoute] int id)
    {
        var query = new GetWeatherQuery
        {
            Id = id
        };
        return Ok(await _mediator.Send(query));
    }
}