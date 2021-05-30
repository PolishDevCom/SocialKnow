using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.AdditionalInfos;

namespace SK.Application.AdditionalInfos.Commands.EditAdditionalInfo
{
    public class EditAdditionalInfoCommandValidator : AbstractValidator<EditAdditionalInfoCommand>
    {
        private readonly IStringLocalizer<AdditionalInfosResource> _localizer;
        public EditAdditionalInfoCommandValidator(IStringLocalizer<AdditionalInfosResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.InfoName).NotEmpty().WithMessage(_localizer["AdditionalInfoValidatorNameEmpty"]);
            RuleFor(a => a.TypeOfField).NotEmpty().WithMessage(_localizer["AdditionalInfoValidatorTypeEmpty"]);
        }
    }
}
