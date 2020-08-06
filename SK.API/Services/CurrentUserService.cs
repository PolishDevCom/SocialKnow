using Microsoft.AspNetCore.Http;
using SK.Application.Common.Interfaces;
using System.Security.Claims;

namespace SK.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId { get { return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); } }
    }
}
