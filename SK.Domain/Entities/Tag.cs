using SK.Domain.Common;
using System;

namespace SK.Domain.Entities
{
    public class Tag : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}