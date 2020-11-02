using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Posts;

namespace SK.Application.Posts.Commands.PinPost
{
    public class PinPostCommandHandler : IRequestHandler<PinPostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<PostsResource> _localizer;

        public PinPostCommandHandler(IApplicationDbContext context, IStringLocalizer<PostsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(PinPostCommand request, CancellationToken cancellationToken)
        {
            var postToPin = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            if (postToPin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostPinError"] });
            }

            postToPin.IsPinned = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostSaveError"] });
        }
    }
}
