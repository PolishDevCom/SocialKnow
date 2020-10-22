using MediatR;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.DeleteDiscussion;
using System;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class DiscussionsController : ApiController
    {
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
    }
}
