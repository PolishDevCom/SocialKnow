using SK.Domain.Common;
using System;

namespace SK.Domain.Entities
{
    public class Article : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
    }
}
