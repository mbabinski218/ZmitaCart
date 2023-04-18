using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetCategories;

public record GetCategoriesWithChildrenBySuperiorIdQuery : IRequest<IEnumerable<CategoryDto>>
{
    public int SuperiorId { get; init; }
    public int ChildrenCount { get; init; }
}

public record GetCategoriesBySuperiorIdQuery : IRequest<IEnumerable<CategoryDto>>
{
    public int SuperiorId { get; init; }
}