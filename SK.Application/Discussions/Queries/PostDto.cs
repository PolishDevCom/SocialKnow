using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Discussions.Queries
{
    public class PostDto : IMapFrom<Post>
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public bool IsPinned { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
