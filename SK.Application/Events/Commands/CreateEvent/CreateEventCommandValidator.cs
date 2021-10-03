using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Events;

namespace SK.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IStringLocalizer<EventsResource> _localizer;

        public CreateEventCommandValidator(IStringLocalizer<EventsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title).NotEmpty().WithMessage(_localizer["EventValidatorTitleEmpty"])
                .MaximumLength(200).WithMessage(_localizer["EventValidatorTitleMaxLength"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(_localizer["EventValidatorDescriptionEmpty"]);
            RuleFor(x => x.Category).NotEmpty().WithMessage(_localizer["EventValidatorCategoryEmpty"]);
            RuleFor(x => x.Date).NotEmpty().WithMessage(_localizer["EventValidatorDateEmpty"]);
            RuleFor(x => x.City).NotEmpty().WithMessage(_localizer["EventValidatorCityEmpty"]);
            RuleFor(x => x.Venue).NotEmpty().WithMessage(_localizer["EventValidatorVenueEmpty"]);
        }
    }
}