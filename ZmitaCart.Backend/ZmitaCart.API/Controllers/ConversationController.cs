using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.ConversationCommands.SendMessage;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

namespace ZmitaCart.API.Controllers;

[Route("conversation")]
[AllowAnonymous]
public class ConversationController : ApiController
{
	public ConversationController(IMediator mediator) : base(mediator)
	{
	}
	
	[HttpPost("message")]
	public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
	{
		return Ok(await mediator.Send(command));
	}
}