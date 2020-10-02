using FluentValidation;

namespace SK.Application.Articles.Commands.EditArticle
{
    public class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
    {
        public EditArticleCommandValidator()
        {
            RuleFor(a => a.Title).NotEmpty();
            RuleFor(a => a.Abstract).NotEmpty();
            RuleFor(a => a.Content).NotEmpty();
        }
    }
}
