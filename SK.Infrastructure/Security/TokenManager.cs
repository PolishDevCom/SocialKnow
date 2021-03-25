using SK.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class TokenManager : ITokenManager
    {
        public async Task DeactivateAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task DeactivateCurrentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            throw new NotImplementedException();
        }
    }
}
