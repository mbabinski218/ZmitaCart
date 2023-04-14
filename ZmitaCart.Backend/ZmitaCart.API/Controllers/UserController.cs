using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.UserCommands.RegisterUser;

namespace ZmitaCart.API.Controllers;

public class UserController: ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
		
	}
	
	[HttpPost("register")]
	public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}

	// [HttpPost("login")]
	// public async Task<ActionResult<string>> Login([FromBody])
	// {
	// 	return Ok();
	// }
}