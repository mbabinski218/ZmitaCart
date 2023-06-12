using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.ConversationCommands.CreateConversation;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

namespace ZmitaCart.API.Controllers;

[Route("conversation")]
public class ConversationController : ApiController
{
	public ConversationController(IMediator mediator) : base(mediator)
	{
	}

	// [HttpGet]
	// [RoleAuthorize]
	// public async Task<ActionResult<IEnumerable<ConversationInfoDto>>> GetAllConversations([FromQuery] GetAllConversationsQuery query)
	// {
	// 	return Ok(await mediator.Send(query));
	// }
	
	[HttpPost]
	[RoleAuthorize]
	public async Task<ActionResult<int>> CreateConversation([FromQuery] CreateConversationCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
}