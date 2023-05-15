using FluentValidation;

namespace ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .NotNull().WithMessage("Category name is required")
            .MaximumLength(50).WithMessage("Category name must be less than 50 characters");
    }
}