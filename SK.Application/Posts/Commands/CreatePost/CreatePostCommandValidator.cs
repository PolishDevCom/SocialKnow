using FluentValidation;

namespace SK.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.Body)
                .NotEmpty().WithMessage("Post body is required.");
            RuleFor(p => p.DiscussionId)
                .NotEmpty().WithMessage("Discussion Id is required.");
        }
    }
}
