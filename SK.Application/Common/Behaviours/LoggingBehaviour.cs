using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var username = _currentUserService.Username ?? string.Empty;
            string userName = string.Empty;

            if (!string.IsNullOrEmpty(username))
            {
                var user = await _identityService.GetUserByUsernameAsync(username);
                userName = user.UserName;
            }

            _logger.LogInformation("SocialKnow Request: {Name} {@UserId} {@UserName} {@Request}", requestName, username, userName, request);
        }
    }
}