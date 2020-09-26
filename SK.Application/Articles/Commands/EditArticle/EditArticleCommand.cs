using MediatR;

namespace SK.Application.TestValues.Commands.EditTestValue
{
    public class EditArticleCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EditArticleCommand() {}
        public EditArticleCommand(int requestId, string requestName)
        {
            Id = requestId;
            Name = requestName;
        }
    }
}
