using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IApplicationDbContext context, IJwtGenerator jwtGenerator, IIdentityService identityService)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _identityService = identityService;
        }

        public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });
            }

            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Username
            };

            var result = await _identityService.CreateUserAsync(user, request.Password);

            if (result.Result.Succeeded)
            {
                return new User
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
            throw new Exception("Problem saving changes");
        }
    }
}
