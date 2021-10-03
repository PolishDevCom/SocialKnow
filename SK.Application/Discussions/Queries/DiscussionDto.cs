using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries
{
    public class DiscussionDto : IMapFrom<Discussion>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPinned { get; set; }
        public bool IsClosed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int NumberOfPosts { get; set; }
        public ICollection<PostDto> Posts { get; set; }
    }
}