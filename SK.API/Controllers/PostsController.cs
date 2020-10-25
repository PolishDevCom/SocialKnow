using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SK.Application.Posts.Commands.CreatePost;
using SK.Application.Posts.Commands.DeletePost;
using MediatR;
using SK.Application.Posts.Commands.EditPost;

namespace SK.API.Controllers
{
    public class PostsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeletePostCommand { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditPostCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
    }
}
