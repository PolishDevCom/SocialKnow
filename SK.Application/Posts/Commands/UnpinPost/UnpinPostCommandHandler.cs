using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SK.Application.Posts.Commands.UnpinPost
{
    public class UnpinPostCommandHandler : IRequestHandler<UnpinPostCommand>
    {
        private readonly IApplicationDbContext _context;

        public UnpinPostCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UnpinPostCommand request, CancellationToken cancellationToken)
        {
            var postToUnpin = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            if (!postToUnpin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Post is unpinned already." });
            }

            postToUnpin.IsPinned = false;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
