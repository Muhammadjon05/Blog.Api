using Blog.Api.DtoModels;
using FluentValidation;

namespace Blog.Api.Validators;

public class PostValidators : AbstractValidator<PostDto>
{
    public PostValidators()
    {
        RuleFor(i => i.Content).NotNull().Length(1, 20);
        RuleFor(i => i.Title).NotNull().Length(1,20);
    }
    
}