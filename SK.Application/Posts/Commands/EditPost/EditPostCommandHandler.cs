using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Posts.Commands.EditPost
{
    public class EditPostCommandHandler : IRequestHandler<EditPostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<PostsResource> _localizer;

        public EditPostCommandHandler(IApplicationDbContext context, IStringLocalizer<PostsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var postToFind = await _context.Posts.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Post), request.Id);

            postToFind.Body = request.Body ?? postToFind.Body;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Post = _localizer["PostSaveError"] });
        }
    }
}