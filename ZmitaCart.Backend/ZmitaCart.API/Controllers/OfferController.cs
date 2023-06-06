using MediatR;
using Microsoft.AspNetCore.Authorization;
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
using ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;
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
        return await mediator.Send(command).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpPut]
    public async Task<ActionResult<int>> UpdateOffer([FromForm] UpdateOfferCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> SearchOffers([FromQuery] SearchOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOffer([FromRoute] int id)
    {
        return await mediator.Send(new DeleteOfferCommand(id)).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<OfferDto>> GetOffer([FromRoute] int id)
    {
        return await mediator.Send(new GetOfferQuery(id)).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpPost("addToFavorites/{id:int}")]
    public async Task<ActionResult> AddToFavorites([FromRoute] int id)
    {
        return await mediator.Send(new AddToFavoritesCommand(id)).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("favorites")]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> GetFavoritesOffers([FromQuery] GetFavouritesOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpPost("buy")]
    public async Task<ActionResult> BuyOffer([FromBody] BuyOfferCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("bought")]
    public async Task<ActionResult<PaginatedList<BoughtOfferDto>>> GetBoughtOffers([FromQuery] GetBoughtOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("byCategoriesName")]
    public async Task<ActionResult<List<OfferInfoWithCategoryNameDto>>> GetOffersByCategoriesNameQuery([FromQuery] GetOffersByCategoriesNameQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
}