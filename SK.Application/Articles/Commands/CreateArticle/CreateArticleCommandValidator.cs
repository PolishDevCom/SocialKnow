using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Articles;

namespace SK.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        private readonly IStringLocalizer<ArticlesResource> _localizer;

        public CreateArticleCommandValidator(IStringLocalizer<ArticlesResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.Title).NotEmpty().WithMessage(_localizer["ArticleValidatorTitleEmpty"]);
            RuleFor(a => a.Abstract).NotEmpty().WithMessage(_localizer["ArticleValidatorAbstractEmpty"]);
            RuleFor(a => a.Content).NotEmpty().WithMessage(_localizer["ArticleValidatorContentEmpty"]);
        }
    }
}