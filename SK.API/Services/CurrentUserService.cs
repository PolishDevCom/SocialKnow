using Microsoft.AspNetCore.Http;
using SK.Application.Common.Interfaces;
using SK.Application.User;
using System.Linq;
using System.Security.Claims;

namespace SK.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Username 
        { 
            get 
            {
                return _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            } 
        }
    }
}
