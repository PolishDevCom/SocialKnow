using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Discussions;

namespace SK.Application.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
    {
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public CreateDiscussionCommandValidator(IStringLocalizer<DiscussionsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer["DiscussionValidatorTitleEmpty"])
                .Length(1, 70).WithMessage(_localizer["DiscussionValidatorTitleLength"]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer["DiscussionValidatorDescriptionEmpty"])
                .Length(1, 200).WithMessage(_localizer["DiscussionValidatorDescriptionLength"]);

            RuleFor(x => x.PostBody)
                .NotEmpty().WithMessage(_localizer["DiscussionValidatorPostEmpty"]);
        }
    }
}
