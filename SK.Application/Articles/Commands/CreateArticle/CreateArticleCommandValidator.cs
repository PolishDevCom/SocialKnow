using FluentValidation;
using SK.Application.Common.Interfaces;

namespace SK.Application.TestValues.Commands.CreateTestValue
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {

            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
        }
    }
}
