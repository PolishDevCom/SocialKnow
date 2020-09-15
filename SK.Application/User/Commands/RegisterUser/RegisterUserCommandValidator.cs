using FluentValidation;

namespace SK.Application.User.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("The username must not be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("The email must not be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not valid email format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password must not be empty");
        }
    }
}
