using MediatR;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.User.Commands.LogoutUser
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly ITokenManager _tokenManager;

        public LogoutUserCommandHandler(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            await _tokenManager.DeactivateCurrentAsync();
            return Unit.Value;
        }
    }
}
