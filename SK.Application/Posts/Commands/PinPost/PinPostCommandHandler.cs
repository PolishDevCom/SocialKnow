using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Posts.Commands.PinPost
{
    public class PinPostCommandHandler : IRequestHandler<PinPostCommand>
    {
        private readonly IApplicationDbContext _context;

        public PinPostCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PinPostCommand request, CancellationToken cancellationToken)
        {
            var postToPin = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            if (postToPin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Post is pinned already." });
            }

            postToPin.IsPinned = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
