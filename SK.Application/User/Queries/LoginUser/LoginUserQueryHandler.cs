using MediatR;
using Microsoft.AspNetCore.Identity;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Domain.Entities;
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

        public LoginUserQueryHandler(IIdentityService identityService, IJwtGenerator jwtGenerator, SignInManager<AppUser> signInManager)
        {
            _jwtGenerator = jwtGenerator;
            _identityService = identityService;
            _signInManager = signInManager;
        }

        public async Task<User> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByEmailAsync(request.Email) ?? throw new RestException(HttpStatusCode.Unauthorized, new { Email = "Not correct email." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if(result.Succeeded)
            {
                return new User
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
            throw new RestException(HttpStatusCode.Unauthorized, "Not correct password.");
        }
    }
}
