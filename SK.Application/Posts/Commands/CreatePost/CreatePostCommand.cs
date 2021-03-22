using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>, IMapTo<Post>
    {
        public Guid Id { get; set; }
        public Guid DiscussionId { get; set; }
        public string Body { get; set; }
        public CreatePostCommand() {}
        public CreatePostCommand(PostCreateDto request)
        {
            Id = request.Id;
            DiscussionId = request.DiscussionId;
            Body = request.Body;
        }
    }
}
