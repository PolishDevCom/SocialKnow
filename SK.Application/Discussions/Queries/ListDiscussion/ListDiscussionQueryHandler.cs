using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Queries.ListDiscussion
{
    public class ListDiscussionQueryHandler : IRequestHandler<ListDiscussionQuery, List<DiscussionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ListDiscussionQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DiscussionDto>> Handle(ListDiscussionQuery request, CancellationToken cancellationToken)
        {
            var pinnedDiscussionList = await _context.Discussions
                .Where(d => d.IsPinned)
                .ProjectTo<DiscussionDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(d => d.Created)
                .ToListAsync(cancellationToken);

            var unpinnedDiscussionList = await _context.Discussions
                .Where(d => d.IsPinned == false)
                .ProjectTo<DiscussionDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(d => d.Created)
                .ToListAsync(cancellationToken);

            var allDiscussionsList = new List<DiscussionDto>(pinnedDiscussionList.Count + unpinnedDiscussionList.Count);
            allDiscussionsList.AddRange(pinnedDiscussionList);
            allDiscussionsList.AddRange(unpinnedDiscussionList);

            allDiscussionsList.ForEach(d => d.NumberOfPosts = d.Posts?.Count ?? 0);

            return allDiscussionsList;
        }
    }
}
