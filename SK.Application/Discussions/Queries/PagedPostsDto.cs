using System;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries
{
    public class PagedPostsDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstsPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalPosts { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        public ICollection<PostDto> Data { get; set; }
    }
}
