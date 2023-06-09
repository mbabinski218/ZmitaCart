using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.ConversationCommands.SendMessage;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.ConversationDtos;
using ZmitaCart.Application.Queries.ConversationQueries.GetAllConversations;

namespace ZmitaCart.API.Controllers;

[Route("conversation")]
public class ConversationController : ApiController
{
	public ConversationController(IMediator mediator) : base(mediator)
	{
	}

	[HttpGet]
	[RoleAuthorize]
	public async Task<ActionResult<PaginatedList<ConversationInfoDto>>> GetAllConversations([FromQuery] GetAllConversationsQuery query)
	{
		return Ok(await mediator.Send(query));
	}
}