using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Common.Models;
using SK.Application.User;
using SK.Application.User.Commands.AddRoleToUser;
using SK.Application.User.Commands.DeleteUser;
using SK.Application.User.Commands.RegisterUser;
using SK.Application.User.Commands.RemoveRoleFromUser;
using SK.Application.User.Queries.GetCurrentUser;
using SK.Application.User.Queries.LoginUser;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Fetches a current logged user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            return Ok(await Mediator.Send(new GetCurrentUserQuery()));
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Deletes a selected user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(DeleteUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Adds new role to selected user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost("role")]
        public async Task<ActionResult> AddRoleToUser(AddRoleToUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes provided role from selected user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("role")]
        public async Task<ActionResult> RemoveRoleFromUser(RemoveRoleFromUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
