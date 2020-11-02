using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries.DetailsDiscussion
{
    public class DetailsDiscussionQueryHandler : IRequestHandler<DetailsDiscussionQuery, DiscussionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DetailsDiscussionQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DiscussionDto> Handle(DetailsDiscussionQuery request, CancellationToken cancellationToken)
        {
            var selectedDiscussionWithPosts = await _context.Discussions
                .Include(d => d.Posts)
                .ProjectTo<DiscussionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == request.Id)
                ??
                throw new NotFoundException(nameof(Discussion), request.Id);

            var pinnedPosts = selectedDiscussionWithPosts.Posts
                .Where(p => p.IsPinned)
                .OrderByDescending(p => p.Created).ToList();

            var unpinnedPosts = selectedDiscussionWithPosts.Posts
                .Where(p => p.IsPinned == false)
                .OrderByDescending(p => p.Created).ToList();

            var orderedPosts = new List<PostDto>(pinnedPosts.Count + unpinnedPosts.Count);
            orderedPosts.AddRange(pinnedPosts);
            orderedPosts.AddRange(unpinnedPosts);

            selectedDiscussionWithPosts.Posts = orderedPosts;
            selectedDiscussionWithPosts.NumberOfPosts = orderedPosts?.Count ?? 0;

            return selectedDiscussionWithPosts;
        }
    }
}
