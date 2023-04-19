using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.UserCommands.AddRoleForUser;
using ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;
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
	public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
	{
		return Ok(await mediator.Send(query));
	}
	
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await mediator.Send(new LogoutUserQuery());
		return Ok();
	}
	
	[HttpPost("addRole")]
	[Authorize(Roles = Role.administrator)]
	public async Task<IActionResult> AddRoleForUser([FromBody] AddRoleForUserCommand command)
	{
		await mediator.Send(command);
		return Ok();
	}
	
	[HttpPost("externalAuthentication")]
	public async Task<IActionResult> ExternalAuthentication([FromBody] ExternalAuthenticationCommand command)
	{
		return Ok(await mediator.Send(command));
	}
}