using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;
using ZmitaCart.Application.Commands.OfferCommands.BuyOffer;
using ZmitaCart.Application.Commands.OfferCommands.CreateOffer;
using ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;
using ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;
using ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffers;
using ZmitaCart.Application.Queries.OfferQueries.GetOffer;
using ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

namespace ZmitaCart.API.Controllers;

[Route("offer")]
public class OfferController : ApiController
{
    public OfferController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateOffer([FromForm] CreateOfferCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPut]
    public async Task<ActionResult<int>> UpdateOffer([FromForm] UpdateOfferCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> SearchOffers([FromQuery] SearchOffersQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteOffer([FromRoute] DeleteOfferCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<OfferDto>> GetOffer([FromRoute] GetOfferQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpPost("addToFavorites/{Id}")]
    public async Task<ActionResult> AddToFavorites([FromRoute] AddToFavoritesCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("favorites")]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> GetFavoritesOffers([FromQuery] GetFavouritesOffersQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpPost("buy")]
    public async Task<ActionResult> BuyOffer([FromBody] BuyOfferCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("bought")]
    public async Task<ActionResult<PaginatedList<BoughtOfferDto>>> GetBoughtOffers([FromQuery] GetBoughtOffersQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}