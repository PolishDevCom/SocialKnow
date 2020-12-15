using MediatR;
using SK.Application.Discussions.Queries;
using System;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
