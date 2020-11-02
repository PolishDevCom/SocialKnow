using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Events;

namespace SK.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IStringLocalizer<EventsResource> _localize;
        public CreateEventCommandValidator(IStringLocalizer<EventsResource> localize)
        {
            _localize = localize;

            RuleFor(x => x.Title).NotEmpty().WithMessage(_localize["EventValidatorTitleEmpty"])
                .MaximumLength(200).WithMessage(_localize["EventValidatorTitleMaxLength"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(_localize["EventValidatorDescriptionEmpty"]);
            RuleFor(x => x.Category).NotEmpty().WithMessage(_localize["EventValidatorCategoryEmpty"]);
            RuleFor(x => x.Date).NotEmpty().WithMessage(_localize["EventValidatorDateEmpty"]);
            RuleFor(x => x.City).NotEmpty().WithMessage(_localize["EventValidatorCityEmpty"]);
            RuleFor(x => x.Venue).NotEmpty().WithMessage(_localize["EventValidatorVenueEmpty"]);
        }
    }
}
