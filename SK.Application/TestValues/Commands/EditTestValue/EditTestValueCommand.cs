using MediatR;

namespace SK.Application.TestValues.Commands.EditTestValue
{
    public class EditTestValueCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EditTestValueCommand() {}
        public EditTestValueCommand(int requestId, string requestName)
        {
            Id = requestId;
            Name = requestName;
        }
    }
}
