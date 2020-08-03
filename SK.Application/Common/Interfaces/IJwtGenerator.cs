using SK.Domain.Entities;

namespace SK.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(AppUser user);
    }
}
