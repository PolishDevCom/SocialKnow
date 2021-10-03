using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SK.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(IDistributedCache cache,
                            IOptions<JwtOptions> jwtOptions,
                            IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _jwtOptions = jwtOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeactivateAsync(string token) => await _cache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
        });

        public async Task DeactivateCurrentAsync()
        {
            await DeactivateAsync(GetCurrentTokenAsync());
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            return await _cache.GetStringAsync(GetKey(token)) == null;
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            return await IsActiveAsync(GetCurrentTokenAsync());
        }

        private string GetCurrentTokenAsync()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token) => $"tokens:{token}: deactivated";
    }
}