using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using SK.Application.Discussions.Queries;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class IsDiscussionOwnerRequirement : IAuthorizationRequirement { }

    public class IsDiscussionOwnerRequirementHandler : AuthorizationHandler<IsDiscussionOwnerRequirement>
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public IsDiscussionOwnerRequirementHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDiscussionOwnerRequirement requirement)
        {
            var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var discussionId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());

            var foundDiscussion = _context.Discussions
                .ProjectTo<DiscussionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == discussionId).Result;

            if (foundDiscussion.CreatedBy == currentUsername)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}