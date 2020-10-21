using FluentValidation;

namespace SK.Application.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
    {
        public CreateDiscussionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Discussion title is required.")
                .Length(1, 70).WithMessage("Discussion title must be at least 1 and at max 70 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Discussion description is required.")
                .Length(1, 200).WithMessage("Discussion description must be at least 1 and at max 200 characters long.");

            RuleFor(x => x.PostBody)
                .NotEmpty().WithMessage("Post body is required.");
        }
    }
}
