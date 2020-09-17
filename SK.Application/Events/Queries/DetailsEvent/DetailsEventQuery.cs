using MediatR;
using System;

namespace SK.Application.Events.Queries.DetailsEvent
{
    public class DetailsEventQuery : IRequest<EventDto>
    {
        public Guid Id { get; set; }
    }
}
