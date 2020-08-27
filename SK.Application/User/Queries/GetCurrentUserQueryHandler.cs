using MediatR;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Queries
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, User>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IIdentityService _identityService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IJwtGenerator jwtGenerator, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _jwtGenerator = jwtGenerator;
            _identityService = identityService;
        }

        public async Task<User> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var username = await _identityService.GetUserByUsernameAsync(_currentUserService.Username);

            return new User
            {
                Username = username.UserName,
                Image = null,
                Token = _jwtGenerator.CreateToken(username)
            };
        }
    }
}
