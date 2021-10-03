using System.Threading.Tasks;

namespace SK.Application.Common.Interfaces
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();

        Task DeactivateCurrentAsync();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateAsync(string token);
    }
}