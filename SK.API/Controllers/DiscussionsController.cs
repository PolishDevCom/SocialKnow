using MediatR;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Discussions.Commands.CloseDiscussion;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.DeleteDiscussion;
using SK.Application.Discussions.Commands.EditDiscussion;
using SK.Application.Discussions.Commands.OpenDiscussion;
using SK.Application.Discussions.Commands.PinDiscussion;
using SK.Application.Discussions.Commands.UnpinDiscussion;
using SK.Application.Discussions.Queries;
using SK.Application.Discussions.Queries.ListDiscussion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class DiscussionsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<DiscussionDto>>> List()
        {
            return await Mediator.Send(new ListDiscussionQuery());
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateDiscussionCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteDiscussionCommand { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditDiscussionCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpPut("{id}/close")]
        public async Task<ActionResult<Unit>> Close(Guid id)
        {
            return await Mediator.Send(new CloseDiscussionCommand { Id = id });
        }

        [HttpPut("{id}/open")]
        public async Task<ActionResult<Unit>> Open(Guid id)
        {
            return await Mediator.Send(new OpenDiscussionCommand { Id = id });
        }

        [HttpPut("{id}/pin")]
        public async Task<ActionResult<Unit>> Pin(Guid id)
        {
            return await Mediator.Send(new PinDiscussionCommand { Id = id });
        }

        [HttpPut("{id}/unpin")]
        public async Task<ActionResult<Unit>> Unpin(Guid id)
        {
            return await Mediator.Send(new UnpinDiscussionCommand { Id = id });
        }
    }
}
