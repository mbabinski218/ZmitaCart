using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<int>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public int? ParentId { get; init; }
}