using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.CreateTag;
using SK.Application.Tags.Commands.EditTag;
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
