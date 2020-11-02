using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SK.Application.Posts.Commands.CreatePost;
using SK.Application.Posts.Commands.DeletePost;
using MediatR;
using SK.Application.Posts.Commands.EditPost;
using SK.Application.Posts.Commands.UnpinPost;
using SK.Application.Posts.Commands.PinPost;
using Microsoft.AspNetCore.Authorization;

namespace SK.API.Controllers
{
    [Authorize]
    public class PostsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "IsPostOwner")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeletePostCommand { Id = id });
        }

        [Authorize(Policy = "IsPostOwner")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditPostCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/pin")]
        public async Task<ActionResult<Unit>> Pin(Guid id)
        {
            return await Mediator.Send(new PinPostCommand { Id = id });
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/unpin")]
        public async Task<ActionResult<Unit>> Unpin(Guid id)
        {
            return await Mediator.Send(new UnpinPostCommand { Id = id });
        }
    }
}
