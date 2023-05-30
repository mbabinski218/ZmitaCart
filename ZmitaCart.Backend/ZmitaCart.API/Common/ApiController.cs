using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Common;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = Role.user)]
[AllowAnonymous]
public class ApiController : ControllerBase
{
	protected readonly IMediator mediator;

	protected ApiController(IMediator mediator)
	{
		this.mediator = mediator;
	}
}