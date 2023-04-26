using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.OfferCommands.CreateOffer;
using ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;
using ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;
using ZmitaCart.Application.Queries.OfferQueries.GetOffer;
using ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategory;

namespace ZmitaCart.API.Controllers;

[Route("offer")]
[AllowAnonymous]
public class OfferController : ApiController
{
    public OfferController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffer([FromForm] CreateOfferCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOffer([FromForm] UpdateOfferCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteOffer([FromRoute] DeleteOfferCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet("byCategory/{CategoryId}")]
    public async Task<IActionResult> GetOffersByCategory([FromRoute] GetOffersByCategoryQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetOffer([FromRoute] GetOfferQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}