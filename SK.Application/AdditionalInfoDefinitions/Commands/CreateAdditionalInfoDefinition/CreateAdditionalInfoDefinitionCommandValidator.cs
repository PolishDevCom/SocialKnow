using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;

namespace SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition
{
    public class CreateAdditionalInfoDefinitionCommandValidator : AbstractValidator<CreateAdditionalInfoDefinitionCommand>
    {
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;

        public CreateAdditionalInfoDefinitionCommandValidator(IStringLocalizer<AdditionalInfoDefinitionsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.InfoName).NotEmpty().WithMessage(_localizer["AdditionalInfoDefinitionValidatorNameEmpty"]);
            RuleFor(a => a.TypeOfField).NotNull().WithMessage(_localizer["AdditionalInfoDefinitionValidatorTypeEmpty"]);
        }
    }
}