using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SK.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class IsEventHostRequirement : IAuthorizationRequirement {}

    public class IsEventHostRequirementHandler : AuthorizationHandler<IsEventHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public IsEventHostRequirementHandler(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEventHostRequirement requirement)
        {
            var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var eventId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());
            var foundEvent = _context.Events.FindAsync(eventId).Result;
            var host = foundEvent.UserEvents.FirstOrDefault(x => x.IsHost);

            if (host?.AppUser?.UserName == currentUsername)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
