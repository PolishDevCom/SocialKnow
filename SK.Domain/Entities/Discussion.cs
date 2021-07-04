using SK.Domain.Common;
using System;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class Discussion : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPinned { get; set; }
        public bool IsClosed { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual Category Category { get; set; }
    }
}
