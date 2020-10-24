using MediatR;
using System;

namespace SK.Application.Discussions.Queries.DetailsDiscussion
{
    public class DetailsDiscussionQuery : IRequest<DiscussionDto>
    {
        public Guid Id { get; set; }
    }
}
