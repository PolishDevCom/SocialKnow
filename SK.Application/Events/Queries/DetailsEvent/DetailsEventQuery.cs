using MediatR;
using System;

namespace SK.Application.Events.Queries.DetailsEvent
{
    public class DetailsEventQuery : IRequest<EventDto>
    {
        public Guid Id { get; set; }

        public DetailsEventQuery() {}
        public DetailsEventQuery(Guid id)
        {
            Id = id;
        }
    }
}
