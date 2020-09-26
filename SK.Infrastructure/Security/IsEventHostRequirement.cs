using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using SK.Application.Events.Queries;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class IsEventHostRequirement : IAuthorizationRequirement {}

    public class IsEventHostRequirementHandler : AuthorizationHandler<IsEventHostRequirement>
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public IsEventHostRequirementHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEventHostRequirement requirement)
        {
            var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var eventId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());
            var foundEvent =  _context.Events
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == eventId).Result;
            var host = foundEvent.UserEvents.FirstOrDefault(x => x.IsHost);

            if (host?.Username == currentUsername)
            context.Succeed(requirement);
            
            return Task.CompletedTask;
        }
    }
}
