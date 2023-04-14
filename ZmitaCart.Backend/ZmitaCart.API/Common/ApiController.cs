using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ZmitaCart.API.Common;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
	protected readonly IMediator _mediator;

	public ApiController(IMediator mediator)
	{
		_mediator = mediator;
	}
}