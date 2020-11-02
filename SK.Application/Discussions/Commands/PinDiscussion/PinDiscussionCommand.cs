using MediatR;
using System;

namespace SK.Application.Discussions.Commands.PinDiscussion
{
    public class PinDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
