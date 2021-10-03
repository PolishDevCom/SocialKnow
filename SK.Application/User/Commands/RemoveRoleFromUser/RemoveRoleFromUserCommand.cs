﻿using MediatR;
using SK.Application.Common.Models;

namespace SK.Application.User.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommand : IRequest<Result>
    {
        public string Username { get; set; }
        public string Role { get; set; }

        public RemoveRoleFromUserCommand()
        {
        }

        public RemoveRoleFromUserCommand(UserAndRoleDto request)
        {
            Username = request.Username;
            Role = request.Role;
        }
    }
}