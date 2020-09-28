using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Commands.EditArticle
{
    public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Article), request.Id);

            article.Id = request.Id;
            article.Title = request.Title;
            article.Abstract = request.Abstract;
            article.Content = request.Content;
            article.Image = request.Image;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Article = "Problem saving changes" });
        }

    }
}
