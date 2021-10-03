using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Tags;

namespace SK.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        private readonly IStringLocalizer<TagsResource> _localizer;

        public CreateTagCommandValidator(IStringLocalizer<TagsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.Title).NotEmpty().WithMessage(_localizer["TagValidatorTitleEmpty"]);
        }
    }
}