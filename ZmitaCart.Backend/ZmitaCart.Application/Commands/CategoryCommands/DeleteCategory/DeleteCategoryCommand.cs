using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;

public record DeleteCategoryCommand : IRequest<Result>
{
    public int Id { get; init; }
}