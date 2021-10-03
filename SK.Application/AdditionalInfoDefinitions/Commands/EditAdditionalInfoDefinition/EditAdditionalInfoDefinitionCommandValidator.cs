using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;

namespace SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition
{
    public class EditAdditionalInfoDefinitionCommandValidator : AbstractValidator<EditAdditionalInfoDefinitionCommand>
    {
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;

        public EditAdditionalInfoDefinitionCommandValidator(IStringLocalizer<AdditionalInfoDefinitionsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.InfoName).NotEmpty().WithMessage(_localizer["AdditionalInfoDefinitionValidatorNameEmpty"]);
            RuleFor(a => a.TypeOfField).NotEmpty().WithMessage(_localizer["AdditionalInfoDefinitionValidatorTypeEmpty"]);
        }
    }
}