using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
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
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public CreateDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
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
                Id = Guid.NewGuid(),
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
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionSaveError"] });
        }
    }
}
