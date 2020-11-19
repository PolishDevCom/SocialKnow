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
using SK.Application.Common.Models;
using System;

namespace SK.Application.Discussions.Queries.DetailsDiscussion
{
    public class DetailsDiscussionQueryHandler : IRequestHandler<DetailsDiscussionQuery, DiscussionWithPagedPostsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public DetailsDiscussionQueryHandler(IApplicationDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<DiscussionWithPagedPostsDto> Handle(DetailsDiscussionQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);

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


            //data for paged posts
            var pagedData = orderedPosts
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = orderedPosts?.Count ?? 0;
            int totalPages = Convert.ToInt32(Math.Ceiling(((double)totalRecords / (double)validFilter.PageSize)));
            var nextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < totalPages
                ? _uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            var previousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= totalPages
                ? _uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            var firstPage = _uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            var lastPage = _uriService.GetPageUri(new PaginationFilter(totalPages, validFilter.PageSize), route);

            var pagedPosts = new PagedPostsDto()
            {
                PageNumber = validFilter.PageNumber,
                PageSize = validFilter.PageSize,
                FirstsPage = firstPage,
                LastPage = lastPage,
                TotalPages = totalPages,
                TotalPosts = totalRecords,
                NextPage = nextPage,
                PreviousPage = previousPage,
                Data = pagedData
            };

            var discussionWithPagedPosts = new DiscussionWithPagedPostsDto()
            {
                Id = selectedDiscussionWithPosts.Id,
                Title = selectedDiscussionWithPosts.Title,
                Description = selectedDiscussionWithPosts.Description,
                IsPinned = selectedDiscussionWithPosts.IsPinned,
                IsClosed = selectedDiscussionWithPosts.IsClosed,
                CreatedBy = selectedDiscussionWithPosts.CreatedBy,
                Created = selectedDiscussionWithPosts.Created,
                LastModifiedBy = selectedDiscussionWithPosts.LastModifiedBy,
                LastModified = selectedDiscussionWithPosts.LastModified,
                NumberOfPosts = totalRecords,
                Posts = pagedPosts
            };

            return discussionWithPagedPosts;
        }
    }
}
