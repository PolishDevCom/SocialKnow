using MediatR;
using System;

namespace SK.Application.Discussions.Commands.OpenDiscussion
{
    public class OpenDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
