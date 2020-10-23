using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Discussions.Commands.CloseDiscussion
{
    public class CloseDiscussionCommandHandler : IRequestHandler<CloseDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;

        public CloseDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CloseDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToClose = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (discussionToClose.IsClosed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Discussion is closed already." });
            }

            discussionToClose.IsClosed = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
