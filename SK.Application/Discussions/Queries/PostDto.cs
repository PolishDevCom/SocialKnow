using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Discussions.Queries
{
    public class PostDto : IMapFrom<Post>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Post content
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Is post pinned
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Post author
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Post creation date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Post modification author
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Post modification date
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
