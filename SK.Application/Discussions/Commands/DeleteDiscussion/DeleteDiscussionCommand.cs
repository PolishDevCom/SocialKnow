using MediatR;
using System;

namespace SK.Application.Discussions.Commands.DeleteDiscussion
{
    public class DeleteDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
