using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var articleToDelete = await _context.Articles.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Article), request.Id);
            _context.Articles.Remove(articleToDelete);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Article = "Problem saving changes" });
        }
    }
}
