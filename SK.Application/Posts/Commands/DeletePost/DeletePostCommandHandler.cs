using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<PostsResource> _localizer;

        public DeletePostCommandHandler(IApplicationDbContext context, IStringLocalizer<PostsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var postToDelete = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            _context.Posts.Remove(postToDelete);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostSaveError"] });
        }
    }
}
