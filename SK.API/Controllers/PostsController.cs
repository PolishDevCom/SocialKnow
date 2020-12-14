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
        /// <summary>
        /// Adds new post.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Deletes a post with selected id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "IsPostOwner")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeletePostCommand { Id = id });
        }

        /// <summary>
        /// Updates an existing post by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = "IsPostOwner")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditPostCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Pins an existing post selected by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/pin")]
        public async Task<ActionResult<Unit>> Pin(Guid id)
        {
            return await Mediator.Send(new PinPostCommand { Id = id });
        }

        /// <summary>
        /// Unpins an existing post selected by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/unpin")]
        public async Task<ActionResult<Unit>> Unpin(Guid id)
        {
            return await Mediator.Send(new UnpinPostCommand { Id = id });
        }
    }
}
