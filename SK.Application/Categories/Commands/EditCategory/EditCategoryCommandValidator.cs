using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Common.Resources.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Categories.Commands.EditCategory
{
    public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
    {
        private readonly IStringLocalizer<CategoriesResource> _localizer;
        public EditCategoryCommandValidator(IStringLocalizer<CategoriesResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.Title).NotEmpty().WithMessage(_localizer["CategoryValidatorTitleEmpty"]);
        }
    }
}
