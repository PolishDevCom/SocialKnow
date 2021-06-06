using System;

namespace SK.Application.Discussions.Commands
{
    public class DiscussionCreateDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostBody { get; set; }
    }
}
