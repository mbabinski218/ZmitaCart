using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.OfferCommands;

namespace ZmitaCart.API.Controllers;

[Route("offer")]
[AllowAnonymous]
public class OfferController : ApiController
{
	public OfferController(IMediator mediator) : base(mediator)
	{
	}
	
	[HttpPost]
	public async Task<IActionResult> CreateOffer([FromBody] CreateOfferCommand command)
	{
		return Ok(await mediator.Send(command));
	}
}