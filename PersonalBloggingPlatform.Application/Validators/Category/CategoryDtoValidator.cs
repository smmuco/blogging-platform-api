using BloggingPlatform.Application.DTOs.Category;
using FluentValidation;

namespace BloggingPlatform.Application.Validators.Category
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name must not exceed 50 characters.");
        }
    }
}
