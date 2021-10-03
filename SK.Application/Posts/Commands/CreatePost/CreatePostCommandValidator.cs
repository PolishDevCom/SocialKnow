using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Posts;

namespace SK.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly IStringLocalizer<PostsResource> _localizer;

        public CreatePostCommandValidator(IStringLocalizer<PostsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(p => p.Body)
                .NotEmpty().WithMessage(_localizer["PostValidatorBodyEmpty"]);
            RuleFor(p => p.DiscussionId)
                .NotEmpty().WithMessage(_localizer["PostValidatorDiscussionIdEmpty"]);
        }
    }
}