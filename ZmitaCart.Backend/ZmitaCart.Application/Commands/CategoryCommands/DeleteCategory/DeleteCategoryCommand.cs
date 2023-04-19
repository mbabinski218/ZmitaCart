using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;

public record DeleteCategoryCommand : IRequest
{
    public int Id { get; init; }
}