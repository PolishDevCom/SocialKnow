using MediatR;
using System;

namespace SK.Application.Posts.Commands.EditPost
{
    public class EditPostCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
    }
}
