using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Discussions.Commands.UnpinDiscussion
{
    public class UnpinDiscussionCommandHandler : IRequestHandler<UnpinDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;

        public UnpinDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UnpinDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToUnpin = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (!discussionToUnpin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Discussion is unpinned already." });
            }

            discussionToUnpin.IsPinned = false;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
