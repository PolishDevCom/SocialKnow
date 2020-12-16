using System;

namespace SK.Application.Posts.Commands
{
    public class PostCreateDto
    {
        public Guid Id { get; set; }
        public Guid DiscussionId { get; set; }
        public string Body { get; set; }
    }
}
