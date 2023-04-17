using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;
using ZmitaCart.Application.Queries.CategoryQueries;

namespace ZmitaCart.API.Controllers;

[Route("category")]
[AllowAnonymous]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand request)
    {
        var response = await mediator.Send(request);
        return Created($"category/{response}", response);
    }

    [HttpGet("getBySuperiorId")]
    public async Task<IActionResult> GetCategoriesBySuperiorId([FromQuery] GetCategoriesBySuperiorIdQuery request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("getFewBySuperiorId")]
    public async Task<IActionResult> GetCategoriesBySuperiorId(
        [FromQuery] GetCategoriesWithChildrenBySuperiorIdQuery request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }
}