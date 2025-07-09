using BloggingPlatform.Application.DTOs.Post;
using FluentValidation;

namespace BloggingPlatform.Application.Validators.Post
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters.");
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category required.")
                .GreaterThan(0).WithMessage("Invalid category.");
            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("Author is required.")
                .GreaterThan(0).WithMessage("Invalid author.");
        }
    }
}
