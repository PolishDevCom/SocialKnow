using SK.Application.Common.Mapping;
using SK.Domain.Entities;

namespace SK.Application.TestValues.Queries
{
    public class TestValueDto : IMapFrom<TestValue>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
