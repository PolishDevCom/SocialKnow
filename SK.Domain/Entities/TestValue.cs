using SK.Domain.Common;

namespace SK.Domain.Entities
{
    public class TestValue : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
