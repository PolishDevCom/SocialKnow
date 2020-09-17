using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.DeleteEvent;
using SK.Application.Events.Commands.EditEvent;
using SK.Application.Events.Commands.SubscribeEvent;
using SK.Application.Events.Commands.UnsubscribeEvent;
using SK.Application.Events.Queries;
using SK.Application.Events.Queries.DetailsEvent;
using SK.Application.Events.Queries.ListEvent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize]
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

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateEventCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsEventHost")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditEventCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsEventHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteEventCommand { Id = id });
        }

        [HttpPost("{id}/subscribe")]
        public async Task<ActionResult<Unit>> Subscribe(Guid id)
        {
            return await Mediator.Send(new SubscribeEventCommand { Id = id });
        }

        [HttpDelete("{id}/subscribe")]
        public async Task<ActionResult<Unit>> Unsubscribe(Guid id)
        {
            return await Mediator.Send(new UnsubscribeEventCommand { Id = id });
        }
    }
}
