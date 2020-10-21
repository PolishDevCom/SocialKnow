using SK.Domain.Common;
using System;

namespace SK.Domain.Entities
{
    public class Post : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public bool IsPinned { get; set; }
        public Guid DiscussionId { get; set; }
        public virtual Discussion Discussion { get; set; }
    }
}
