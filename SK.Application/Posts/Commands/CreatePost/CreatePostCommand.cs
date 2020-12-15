using MediatR;
using SK.Application.Discussions.Queries;
using System;

namespace SK.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public Guid DiscussionId { get; set; }
        public string Body { get; set; }
    }
}
