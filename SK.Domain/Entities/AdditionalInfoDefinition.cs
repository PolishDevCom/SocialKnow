using SK.Domain.Common;
using System;

namespace SK.Domain.Entities
{
    public class AdditionalInfoDefinition : AuditableEntity
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public string InfoType { get; set; }
    }
}
