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
    public class IsPostOwnerRequirement : IAuthorizationRequirement { }

    public class IsPostOwnerRequirementHandler : AuthorizationHandler<IsPostOwnerRequirement>
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public IsPostOwnerRequirementHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostOwnerRequirement requirement)
        {
            var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var postId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value.ToString());

            var foundPost = _context.Events
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == postId).Result;

            if (foundPost.CreatedBy == currentUsername)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
