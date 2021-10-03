using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Users;

namespace SK.Application.User.Queries.LoginUser
{
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        private readonly IStringLocalizer<UsersResource> _localizer;

        public LoginUserQueryValidator(IStringLocalizer<UsersResource> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Email).NotEmpty().WithMessage(_localizer["UserValidatorEmailEmpty"]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(_localizer["UserValidatorPasswordEmpty"]);
        }
    }
}