using MediatR;
using Microsoft.AspNetCore.Identity;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Commands.AddRoleToUser
{
    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleToUserCommandHandler(IIdentityService identityService, RoleManager<IdentityRole> roleManager)
        {
            _identityService = identityService;
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByUsernameAsync(request.Username) ?? throw new NotFoundException(nameof(AppUser), request.Username);

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Role = "Provided role does not exist" });
            }

            var result = await _identityService.AddRoleToUserAsync(user, request.Role);

            return result;
        }
    }
}
