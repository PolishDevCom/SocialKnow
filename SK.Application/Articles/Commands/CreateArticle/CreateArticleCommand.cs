using MediatR;

namespace SK.Application.TestValues.Commands.CreateTestValue
{
    public class CreateArticleCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CreateArticleCommand() {}
        public CreateArticleCommand(int requestId, string requestName)
        {
            Id = requestId;
            Name = requestName;
        }
    }
}