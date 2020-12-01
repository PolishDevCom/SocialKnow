using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Users;
using SK.Domain.Entities;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, User>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IIdentityService _identityService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStringLocalizer<UsersResource> _localizer;

        public LoginUserQueryHandler(IIdentityService identityService, IJwtGenerator jwtGenerator, SignInManager<AppUser> signInManager, IStringLocalizer<UsersResource> localizer)
        {
            _jwtGenerator = jwtGenerator;
            _identityService = identityService;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        public async Task<User> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByEmailAsync(request.Email) ?? throw new RestException(HttpStatusCode.Unauthorized, new { Email = _localizer["UserLoginEmailError"] });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if(result.Succeeded)
            {
                return new User
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }
            throw new RestException(HttpStatusCode.Unauthorized, new { Password = _localizer["UserLoginPasswordError"] });
        }
    }
}
