using FluentValidation;

namespace SK.Application.Posts.Commands.EditPost
{
    public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
    {
        public EditPostCommandValidator()
        {
            RuleFor(p => p.Body)
                .NotEmpty().WithMessage("Post body is required.");
        }
    }
}
