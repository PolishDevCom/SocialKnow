using FluentValidation;

namespace SK.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(a => a.Title).NotEmpty();
            RuleFor(a => a.Abstract).NotEmpty();
            RuleFor(a => a.Content).NotEmpty();
        }
    }
}
