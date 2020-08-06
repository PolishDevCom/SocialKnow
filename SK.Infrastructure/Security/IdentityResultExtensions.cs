using Microsoft.AspNetCore.Identity;
using SK.Application.Common.Models;
using System.Linq;

namespace SK.Infrastructure.Security
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
