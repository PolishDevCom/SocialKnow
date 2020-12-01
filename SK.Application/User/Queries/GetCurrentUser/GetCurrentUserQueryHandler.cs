using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Queries.GetCurrentUser
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
            var user = await _identityService.GetUserByUsernameAsync(_currentUserService.Username) ?? throw new NotFoundException(nameof(AppUser), _currentUserService.Username);

            return new User
            {
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _jwtGenerator.CreateToken(user)
            };
        }
    }
}
