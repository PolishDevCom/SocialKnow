using FluentValidation;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommandValidator : AbstractValidator<EditDiscussionCommand>
    {
        public EditDiscussionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Discussion title is required.")
                .Length(1, 70).WithMessage("Discussion title must be at least 1 and at max 70 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Discussion description is required.")
                .Length(1, 200).WithMessage("Discussion description must be at least 1 and at max 200 characters long.");
        }
    }
}
