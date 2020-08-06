using MediatR;

namespace SK.Application.TestValues.Queries.DetailsTestValue
{
    public class DetailsTestValueQuery : IRequest<TestValueDto>
    {
        public int Id { get; set; }
    }
}
