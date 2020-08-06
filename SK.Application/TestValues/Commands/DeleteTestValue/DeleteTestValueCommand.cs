using MediatR;

namespace SK.Application.TestValues.Commands.DeleteTestValue
{
    public class DeleteTestValueCommand : IRequest
    {
        public int Id { get; set; }
    }
}
