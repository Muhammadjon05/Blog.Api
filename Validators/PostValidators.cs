using Blog.Api.DtoModels;
using FluentValidation;

namespace Blog.Api.Validators;

public class PostValidators : AbstractValidator<PostDto>
{
    public PostValidators()
    {
        RuleFor(i => i.Content).NotNull().Length(4, 20);
        RuleFor(i => i.Title).NotNull().Length(4,20);
    }
    
}