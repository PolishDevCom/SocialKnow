using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Profiles;

namespace SK.Application.Profiles.Commands.EditProfile
{
    public class EditProfileValidator : AbstractValidator<EditProfileCommand>
    {
        private readonly IStringLocalizer<ProfilesResource> _localizer;

        public EditProfileValidator(IStringLocalizer<ProfilesResource> localizer)
        {
            _localizer = localizer;

            RuleFor(p => p.Nickname).NotEmpty().WithMessage(_localizer["ProfileValidatorNicknameEmpty"]);
            RuleFor(p => p.Age).NotEmpty().WithMessage(_localizer["ProfileValidatorGenderEmpty"]);
        }
    }
}