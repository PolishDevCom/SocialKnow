using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.CreateTag;
using SK.Application.Tags.Commands.EditTag;
using SK.Application.Tags.Queries;
using SK.Application.Tags.Queries.ListTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TagsController : ApiController
    {
        /// <summary>
        /// Fetches lists of tags with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns>List of tags</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<TagDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListTagQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Adds a new tag.
        /// </summary>
        /// <param name="newTag">New tag</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] TagCreateOrEditDto newTag)
        {
            return Ok(await Mediator.Send(new CreateTagCommand(newTag)));
        }

        /// <summary>
        /// Updates an existing tag selected by id.
        /// </summary>
        /// <param name="editTag">Edited tag</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] TagCreateOrEditDto editTag)
        {
            await Mediator.Send(new EditTagCommand(editTag));
            return NoContent();
        }
    }
}
