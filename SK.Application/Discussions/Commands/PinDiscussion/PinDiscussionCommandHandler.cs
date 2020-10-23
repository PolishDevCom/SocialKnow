using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Discussions.Commands.PinDiscussion
{
    public class PinDiscussionCommandHandler : IRequestHandler<PinDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;

        public PinDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PinDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToPin = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (discussionToPin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Discussion is pinned already." });
            }

            discussionToPin.IsPinned = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
