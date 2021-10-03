using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Users;

namespace SK.Application.User.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IStringLocalizer<UsersResource> _localizer;

        public RegisterUserCommandValidator(IStringLocalizer<UsersResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Username).NotEmpty().WithMessage(_localizer["UserValidatorUsernameEmpty"]);
            RuleFor(x => x.Email).NotEmpty().WithMessage(_localizer["UserValidatorEmailEmpty"]);
            RuleFor(x => x.Email).EmailAddress().WithMessage(_localizer["UserValidatorEmailFormat"]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(_localizer["UserValidatorPasswordEmpty"]);
        }
    }
}