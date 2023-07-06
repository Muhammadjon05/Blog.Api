using Blog.Api.DtoModels;
using FluentValidation;

namespace Blog.Api.Validators;

public class BlogValidator : AbstractValidator<BlogDto>
{
    public BlogValidator()
    {
        RuleFor(u => u.BlogName).NotNull().Length(3, 15);
    }
}