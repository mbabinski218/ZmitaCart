using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.ConversationCommands.CreateConversation;

namespace ZmitaCart.API.Controllers;

[Route("conversation")]
public class ConversationController : ApiController
{
	public ConversationController(IMediator mediator) : base(mediator)
	{
	}
	
	[HttpPost]
	[RoleAuthorize]
	public async Task<ActionResult<int>> CreateConversation([FromQuery] CreateConversationCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
}