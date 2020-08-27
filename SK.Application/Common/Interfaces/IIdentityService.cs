using SK.Application.Common.Models;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}

