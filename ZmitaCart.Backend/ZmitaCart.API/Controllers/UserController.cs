using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.UserCommands.AddRole;
using ZmitaCart.Application.Commands.UserCommands.DeleteFeedback;
using ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;
using ZmitaCart.Application.Commands.UserCommands.GiveFeedback;
using ZmitaCart.Application.Commands.UserCommands.Register;
using ZmitaCart.Application.Commands.UserCommands.UpdateCredentials;
using ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Queries.UserQueries.GetFeedback;
using ZmitaCart.Application.Queries.UserQueries.LoginUser;
using ZmitaCart.Application.Queries.UserQueries.LogoutUser;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Controllers;

[Route("user")]
public class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
		
	}
	
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}

	[HttpGet("login")]
	[AllowAnonymous]
	public async Task<ActionResult<string>> Login([FromQuery] LoginUserQuery query)
	{
		return Ok(await mediator.Send(query));
	}
	
	[HttpPost("logout")]
	public async Task<ActionResult> Logout()
	{
		await mediator.Send(new LogoutUserQuery());
		return Ok();
	}
	
	[HttpPost("addRole")]
	[Authorize(Roles = Role.administrator)]
	public async Task<ActionResult> AddRoleForUser([FromBody] AddRoleForUserCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}
	
	[HttpPost("externalAuthentication")]
	public async Task<ActionResult<string>> ExternalAuthentication([FromBody] ExternalAuthenticationCommand command)
	{
		return Ok(await mediator.Send(command));
	}
	
	[HttpPut("updateCredentials")]
	public async Task<ActionResult> UpdateCredentials([FromBody] UpdateCredentialsCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}
	
	[HttpPost("feedback")]
	public async Task<ActionResult<int>> GiveFeedback([FromBody] GiveFeedbackCommand command)
	{
		return Ok(await mediator.Send(command));
	}
	
	[HttpPut("feedback")]
	public async Task<ActionResult<int>> UpdateFeedback([FromBody] UpdateFeedbackCommand command)
	{
		return Ok(await mediator.Send(command));
	}
	
	[HttpDelete("feedback/{Id}")]
	public async Task<ActionResult> DeleteFeedback([FromRoute] DeleteFeedbackCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}
	
	[HttpGet("feedback")]
	public async Task<ActionResult<PaginatedList<FeedbackDto>>> GetFeedback([FromQuery] GetFeedbackQuery query)
	{
		return Ok(await mediator.Send(query));
	}
}