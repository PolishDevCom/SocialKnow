using MediatR;
using System;

namespace SK.Application.Discussions.Commands.CloseDiscussion
{
    public class CloseDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }

        public CloseDiscussionCommand()
        {
        }

        public CloseDiscussionCommand(Guid id)
        {
            Id = id;
        }
    }
}