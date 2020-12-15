using MediatR;
using SK.Application.Common.Models;

namespace SK.Application.User.Commands.AddRoleToUser
{
    public class AddRoleToUserCommand : IRequest<Result>
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public AddRoleToUserCommand() {}
        public AddRoleToUserCommand(UserRoleDto request)
        {
            Username = request.Username;
            Role = request.Role;
        }
    }
}
