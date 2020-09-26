using MediatR;

namespace SK.Application.TestValues.Commands.DeleteTestValue
{
    public class DeleteArticleCommand : IRequest
    {
        public int Id { get; set; }
    }
}
