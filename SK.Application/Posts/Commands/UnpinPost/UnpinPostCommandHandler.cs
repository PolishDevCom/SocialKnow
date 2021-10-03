using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Posts.Commands.UnpinPost
{
    public class UnpinPostCommandHandler : IRequestHandler<UnpinPostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<PostsResource> _localizer;

        public UnpinPostCommandHandler(IApplicationDbContext context, IStringLocalizer<PostsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UnpinPostCommand request, CancellationToken cancellationToken)
        {
            var postToUnpin = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            if (!postToUnpin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostUnpinError"] });
            }

            postToUnpin.IsPinned = false;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostSaveError"] });
        }
    }
}