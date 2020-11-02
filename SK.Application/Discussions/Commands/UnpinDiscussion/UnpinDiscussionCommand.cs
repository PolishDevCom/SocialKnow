using MediatR;
using System;

namespace SK.Application.Discussions.Commands.UnpinDiscussion
{
    public class UnpinDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
