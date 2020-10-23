using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Discussions.Commands.OpenDiscussion
{
    public class OpenDiscussionCommandHandler : IRequestHandler<OpenDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;

        public OpenDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(OpenDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToOpen = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (!discussionToOpen.IsClosed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Discussion is opened already." });
            }

            discussionToOpen.IsClosed = false;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
