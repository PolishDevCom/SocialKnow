using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Resources.Users;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<UsersResource> _localizer;

        public DeleteUserCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService, IStringLocalizer<UsersResource> localizer)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByUsernameAsync(request.Username) ?? throw new NotFoundException(nameof(AppUser), request.Username);

            if (user.UserName != _currentUserService.Username)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Username = _localizer["UserDeleteNotAuthorizeError"] });
            }

            var result = await _identityService.DeleteUserAsync(user.UserName);

            return result;
        }
    }
}
