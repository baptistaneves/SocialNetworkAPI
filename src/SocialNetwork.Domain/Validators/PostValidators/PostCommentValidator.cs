using FluentValidation;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Domain.Validators.PostValidators
{
    internal class PostCommentValidator : AbstractValidator<PostComment>
    {
        public PostCommentValidator()
        {
            RuleFor(pc => pc.Text)
              .NotNull().WithMessage("Comment text should not be null")
              .NotEmpty().WithMessage("Comment text should not be empty.")
              .MinimumLength(1).WithMessage("Comment text should have at least 1")
              .MaximumLength(1000);
        }
    }
}
