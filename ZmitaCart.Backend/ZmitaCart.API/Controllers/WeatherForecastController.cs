using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.WeatherForecastCommands.CreateWeather;
using ZmitaCart.Application.Queries.WeatherForecast.GetWeather;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.API.Controllers;

public class WeatherForecastController : ApiController
{
    public WeatherForecastController(IMediator mediator) : base(mediator)
    {
        
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