using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;
using ZmitaCart.Application.Queries.CategoryQueries.GetAllSuperiors;
using ZmitaCart.Application.Queries.CategoryQueries.GetCategories;
using ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;

namespace ZmitaCart.API.Controllers;

[Route("category")]
[AllowAnonymous]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] Application.Commands.CategoryCommands.CreateCategory.CreateCategoryCommand request)
    {
        var response = await mediator.Send(request);
        return Created($"category/{response}", response);
    }

    [HttpGet("getBySuperiorId")]
    public async Task<IActionResult> GetCategoriesBySuperiorId([FromQuery] GetCategoriesBySuperiorIdQuery request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpGet("getFewBySuperiorId")]
    public async Task<IActionResult> GetCategoriesBySuperiorId(
        [FromQuery] GetCategoriesWithChildrenBySuperiorIdQuery request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpGet("getAllSuperiors")]
    public async Task<IActionResult> GetCategoriesBySuperiorId()
    {
        return Ok(await mediator.Send(new GetAllSuperiorsQuery()));
    }

    [HttpPut("updateCategory")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}