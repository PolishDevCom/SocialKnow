using MediatR;
using System;

namespace SK.Application.Discussions.Commands.UnpinDiscussion
{
    public class UnpinDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }

        public UnpinDiscussionCommand() {}
        public UnpinDiscussionCommand(Guid id)
        {
            Id = id;
        }
    }
}
