using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Posts;

namespace SK.Application.Posts.Commands.EditPost
{
    public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
    {
        private readonly IStringLocalizer<PostsResource> _localizer;

        public EditPostCommandValidator(IStringLocalizer<PostsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(p => p.Body)
                .NotEmpty().WithMessage(_localizer["PostValidatorBodyEmpty"]);
        }
    }
}