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
	
	[HttpPost("message")]
	public async Task<ActionResult<int>> SendMessage([FromBody] SendMessageCommand command)
	{
		return Ok(await mediator.Send(command));
	}
	
	[HttpGet]
	public async Task<ActionResult<PaginatedList<ConversationInfoDto>>> GetAllConversations([FromQuery] GetAllConversationsQuery query)
	{
		return Ok(await mediator.Send(query));
	}
}