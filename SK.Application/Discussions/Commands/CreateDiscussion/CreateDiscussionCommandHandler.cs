using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommandHandler : IRequestHandler<CreateDiscussionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var newDiscussion = new Discussion
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                IsClosed = false,
                IsPinned = false
            };

            await _context.Discussions.AddAsync(newDiscussion);

            var firstPost = new Post
            {
                Body = request.PostBody,
                IsPinned = false,
                DiscussionId = newDiscussion.Id,
                Discussion = newDiscussion
            };

            await _context.Posts.AddAsync(firstPost);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return newDiscussion.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });
        }
    }
}
