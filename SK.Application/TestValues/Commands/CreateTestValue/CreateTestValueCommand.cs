using MediatR;

namespace SK.Application.TestValues.Commands.CreateTestValue
{
    public class CreateTestValueCommand : IRequest<int>
    {
            public int Id { get; set; }
            public string Name { get; set; }
    }
}