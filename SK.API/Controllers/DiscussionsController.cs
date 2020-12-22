using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Application.Discussions.Commands;
using SK.Application.Discussions.Commands.CloseDiscussion;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.DeleteDiscussion;
using SK.Application.Discussions.Commands.EditDiscussion;
using SK.Application.Discussions.Commands.OpenDiscussion;
using SK.Application.Discussions.Commands.PinDiscussion;
using SK.Application.Discussions.Commands.UnpinDiscussion;
using SK.Application.Discussions.Queries;
using SK.Application.Discussions.Queries.DetailsDiscussion;
using SK.Application.Discussions.Queries.ListDiscussion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize]
    public class DiscussionsController : ApiController
    {
        /// <summary>
        /// Fetches lists of discussions with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<DiscussionDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListDiscussionQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Fetches a single discussion by id with releted posts filtered pagination filter.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <param name="paginationFilter">Posts pagination filter</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscussionWithPagedPostsDto>> Details(Guid id, [FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new DetailsDiscussionQuery(id, paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Adds a new discussion.
        /// </summary>
        /// <param name="newDiscussion">New discussion</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] DiscussionCreateDto newDiscussion)
        {
            return Ok(await Mediator.Send(new CreateDiscussionCommand(newDiscussion)));
        }

        /// <summary>
        /// Deletes a discussion with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteDiscussionCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Updates an existing discussion selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <param name="editedDiscussion">Edited discussion</param>
        /// <returns></returns>
        [Authorize(Policy = "IsDiscussionOwner")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, [FromBody] DiscussionEditDto editedDiscussion)
        {
            var command = new EditDiscussionCommand(editedDiscussion)
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Closes an existing open discussion selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/close")]
        public async Task<ActionResult> Close(Guid id)
        {
            await Mediator.Send(new CloseDiscussionCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Opens an existing closed discussion selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/open")]
        public async Task<ActionResult> Open(Guid id)
        {
            await Mediator.Send(new OpenDiscussionCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Pins an existing discussion selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/pin")]
        public async Task<ActionResult> Pin(Guid id)
        {
            await Mediator.Send(new PinDiscussionCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Unpins an existing discussion selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Discussion ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/unpin")]
        public async Task<ActionResult> Unpin(Guid id)
        {
            await Mediator.Send(new UnpinDiscussionCommand(id));
            return NoContent();
        }
    }
}
