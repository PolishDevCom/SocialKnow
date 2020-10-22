using Microsoft.AspNetCore.Mvc;
using SK.Application.Discussions.Commands.CreateDiscussion;
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
    }
}
