using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.ConversationCommands.SendMessage;

namespace ZmitaCart.API.Controllers;

[Microsoft.AspNetCore.Components.Route("conversation")]
[AllowAnonymous]
public class ConversationController : ApiController
{
	protected ConversationController(IMediator mediator) : base(mediator)
	{
	}
	
	// [HttpPost]
	// public async Task<IActionResult> JoinGroup([FromBody] JoinGroupCommand command)
	// {
	// 	return Ok(await mediator.Send(command));
	// }
	
	[HttpPost]
	public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
	{
		return Ok(await mediator.Send(command));
	}
}