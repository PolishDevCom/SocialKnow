using SK.Domain.Common;
using System;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
    }
}