using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.User;
using SK.Application.User.Commands;
using SK.Application.User.Commands.AddRoleToUser;
using SK.Application.User.Commands.DeleteUser;
using SK.Application.User.Commands.RegisterUser;
using SK.Application.User.Commands.RemoveRoleFromUser;
using SK.Application.User.Queries;
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
        /// <param name="registerCredentials">User register credentials</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserDto registerCredentials)
        {
            return Ok(await Mediator.Send(new RegisterUserCommand(registerCredentials)));
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="loginCredentials">User login credentials</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUserDto loginCredentials)
        {
            return Ok(await Mediator.Send(new LoginUserQuery(loginCredentials)));
        }

        /// <summary>
        /// Deletes a selected user by username.
        /// </summary>
        /// <param name="username" example="Tom123">Username</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string username)
        {
            await Mediator.Send(new DeleteUserCommand(username));
            return NoContent();
        }

        /// <summary>
        /// Adds new role to selected user.
        /// </summary>
        /// <param name="userRole">User and role to add</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost("role")]
        public async Task<ActionResult> AddRoleToUser(UserAndRoleDto userRole)
        {
            await Mediator.Send(new AddRoleToUserCommand(userRole));
            return NoContent();
        }

        /// <summary>
        /// Deletes provided role from selected user.
        /// </summary>
        /// <param name="userRole">User and role to delete</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("role")]
        public async Task<ActionResult> RemoveRoleFromUser(UserAndRoleDto userRole)
        {
            await Mediator.Send(new RemoveRoleFromUserCommand(userRole));
            return NoContent();
        }
    }
}
