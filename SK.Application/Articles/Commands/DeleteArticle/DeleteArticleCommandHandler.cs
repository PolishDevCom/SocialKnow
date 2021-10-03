using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ArticlesResource> _localizer;

        public DeleteArticleCommandHandler(IApplicationDbContext context, IStringLocalizer<ArticlesResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var articleToDelete = await _context.Articles.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Article), request.Id);
            _context.Articles.Remove(articleToDelete);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Article = _localizer["ArticleSaveError"] });
        }
    }
}