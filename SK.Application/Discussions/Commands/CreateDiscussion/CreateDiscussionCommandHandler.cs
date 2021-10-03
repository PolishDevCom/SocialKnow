using AutoMapper;
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
        private readonly IMapper _mapper;

        public CreateDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var newDiscussion = _mapper.Map<Discussion>(request);

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category != null)
            {
                newDiscussion.Category = category;
            }

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