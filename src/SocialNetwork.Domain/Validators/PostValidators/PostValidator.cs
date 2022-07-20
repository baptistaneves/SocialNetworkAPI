using FluentValidation;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Domain.Validators.PostValidators
{
    internal class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(p => p.TextContent)
                .NotNull().WithMessage("Post text content should can't be null")
              .NotEmpty().WithMessage("Post text content should can't be empty");
        }
    }
}
