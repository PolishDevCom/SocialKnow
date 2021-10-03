using Microsoft.AspNetCore.Http;
using SK.Application.Common.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace SK.API.Middlewares
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}