using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Discussions;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommandValidator : AbstractValidator<EditDiscussionCommand>
    {
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public EditDiscussionCommandValidator(IStringLocalizer<DiscussionsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer["DiscussionValidatorTitleEmpty"])
                .Length(1, 70).WithMessage(_localizer["DiscussionValidatorTitleLength"]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer["DiscussionValidatorDescriptionEmpty"])
                .Length(1, 200).WithMessage(_localizer["DiscussionValidatorDescriptionLength"]);
        }
    }
}