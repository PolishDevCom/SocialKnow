using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Categories;

namespace SK.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IStringLocalizer<CategoriesResource> _localizer;

        public CreateCategoryCommandValidator(IStringLocalizer<CategoriesResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.Title).NotEmpty().WithMessage(_localizer["CategoryValidatorTitleEmpty"]);
        }
    }
}