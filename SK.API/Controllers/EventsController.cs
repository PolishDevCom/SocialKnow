using Microsoft.AspNetCore.Mvc;
using SK.Application.Events.Queries;
using SK.Application.Events.Queries.DetailsEvent;
using SK.Application.Events.Queries.ListEvent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class EventsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> List()
        {
            return await Mediator.Send(new ListEventQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> Details(Guid id)
        {
            return await Mediator.Send(new DetailsEventQuery { Id = id });
        }
    }
}
