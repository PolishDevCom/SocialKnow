using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> CreateToken(AppUser user);
    }
}