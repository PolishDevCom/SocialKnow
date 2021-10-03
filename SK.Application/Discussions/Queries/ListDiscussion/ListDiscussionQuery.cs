using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries.ListDiscussion
{
    public class ListDiscussionQuery : IRequest<PagedResponse<List<DiscussionDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListDiscussionQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}