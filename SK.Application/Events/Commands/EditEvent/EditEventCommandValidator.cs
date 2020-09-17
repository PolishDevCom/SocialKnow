using FluentValidation;

namespace SK.Application.Events.Commands.EditEvent
{
    public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
    {
        public EditEventCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }
    }
}
