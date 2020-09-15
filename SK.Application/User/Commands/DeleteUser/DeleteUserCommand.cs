using MediatR;
using SK.Application.Common.Models;

namespace SK.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public string Username { get; set; }
    }
}
