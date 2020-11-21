using MediatR;
using SK.Application.Common.Models;
using System;

namespace SK.Application.Discussions.Queries.DetailsDiscussion
{
    public class DetailsDiscussionQuery : IRequest<DiscussionWithPagedPostsDto>
    {
        public Guid Id { get; set; }
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }
        public DetailsDiscussionQuery() {}

        public DetailsDiscussionQuery(Guid id, PaginationFilter filter, string path)
        {
            Id = id;
            Filter = filter;
            Path = path;
        }
    }
}
