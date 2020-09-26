using MediatR;
using System.Collections.Generic;

namespace SK.Application.Events.Queries.ListEvent
{
    public class ListEventQuery : IRequest<List<EventDto>> {}
}
