using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.UserCommands.AddRoleForUser;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;
using ZmitaCart.Application.Queries.UserQueries.LoginUser;
using ZmitaCart.Application.Queries.UserQueries.LogoutUser;

namespace ZmitaCart.API.Controllers;

[Route("user")]
public class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
		
	}
	
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
	{
		return Ok(await _mediator.Send(query));
	}
	
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await _mediator.Send(new LogoutUserQuery());
		return Ok();
	}
	
	[HttpPost("addRole")]
	public async Task<IActionResult> AddRoleForUser([FromBody] AddRoleForUserCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}
}