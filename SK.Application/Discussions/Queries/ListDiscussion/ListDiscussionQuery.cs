using MediatR;
using System.Collections.Generic;

namespace SK.Application.Discussions.Queries.ListDiscussion
{
    public class ListDiscussionQuery : IRequest<List<DiscussionDto>> { }
}
