using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Application.Events.Commands;
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
        /// <summary>
        /// Fetches list of events with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<EventDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListEventQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Fetches a single event by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Event ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> Details(Guid id)
        {
            return Ok(await Mediator.Send(new DetailsEventQuery(id)));
        }

        /// <summary>
        /// Adds new event.
        /// </summary>
        /// <param name="newEvent">New event</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] EventCreateOrEditDto newEvent)
        {
            return Ok(await Mediator.Send(new CreateEventCommand(newEvent)));
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="editedEvent">Edited event</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "IsEventHost")]
        public async Task<ActionResult> Edit([FromBody] EventCreateOrEditDto editedEvent)
        {
            await Mediator.Send(new EditEventCommand(editedEvent));
            return NoContent();
        }

        /// <summary>
        /// Deletes an event with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Event ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "IsEventHost")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteEventCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Subscribes user to event with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Event ID</param>
        /// <returns></returns>
        [HttpPost("{id}/subscribe")]
        public async Task<ActionResult> Subscribe(Guid id)
        {
            await Mediator.Send(new SubscribeEventCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Unsubscribe user from event with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Event ID</param>
        /// <returns></returns>
        [HttpDelete("{id}/subscribe")]
        public async Task<ActionResult> Unsubscribe(Guid id)
        {
            await Mediator.Send(new UnsubscribeEventCommand(id));
            return NoContent();
        }
    }
}