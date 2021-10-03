using System;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries
{
    public class PagedPostsDto
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// URI to first page
        /// </summary>
        public Uri FirstsPage { get; set; }

        /// <summary>
        /// URI to last page
        /// </summary>
        public Uri LastPage { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total number of posts
        /// </summary>
        public int TotalPosts { get; set; }

        /// <summary>
        /// URI to next page
        /// </summary>
        public Uri NextPage { get; set; }

        /// <summary>
        /// URI to previous page
        /// </summary>
        public Uri PreviousPage { get; set; }

        /// <summary>
        /// Collection of posts
        /// </summary>
        public ICollection<PostDto> Data { get; set; }
    }
}