using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreatePostCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var discussionToFind = await _context.Discussions.FindAsync(request.DiscussionId) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (discussionToFind.IsClosed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Post = "Adding post to closed discussion is not allowed." });
            }

            var newPost = new Post()
            {
                Id = request.Id,
                Body = request.Body,
                IsPinned = false,
                DiscussionId = request.DiscussionId
            };

            await _context.Posts.AddAsync(newPost);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return newPost.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });
        }
    }
}
