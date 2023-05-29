using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;
using ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;
using ZmitaCart.Application.Queries.CategoryQueries.GetAllSuperiors;
using ZmitaCart.Application.Queries.CategoryQueries.GetCategories;
using ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.API.Controllers;

[Route("category")]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Authorize(Roles = Role.administrator)]
    public async Task<ActionResult<int>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        return await mediator.Send(command).Then(
            s => Created($"category/{s.Value}", s.Value),
            err => BadRequest(err.ToList()));
    }

    [HttpGet("getBySuperiorId")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesBySuperiorId([FromQuery] GetCategoriesBySuperiorIdQuery request)
    {
        return await mediator.Send(request).Then( 
            s => Ok(s.Value), 
            err => BadRequest(err.ToList()));
    }

    [HttpGet("getFewBySuperiorId")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesBySuperiorId([FromQuery] GetCategoriesWithChildrenBySuperiorIdQuery request)
    {
        return await mediator.Send(request).Then( 
            s => Ok(s.Value), 
            err => BadRequest(err.ToList()));
    }

    [HttpGet("getAllSuperiors")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SuperiorCategoryDto>>> GetCategoriesBySuperiorId()
    {
        return await mediator.Send(new GetAllSuperiorsQuery()).Then(
            s => Ok(s.Value), 
            err => BadRequest(err.ToList()));
    }

    [HttpPut("updateCategory")]
    [Authorize(Roles = Role.administrator)]
    public async Task<ActionResult<int>> UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(s.Value), 
            err => BadRequest(err.ToList()));
    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = Role.administrator)]
    public async Task<ActionResult> DeleteCategory([FromRoute] DeleteCategoryCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(),
            err => BadRequest(err.ToList()));
    }
}