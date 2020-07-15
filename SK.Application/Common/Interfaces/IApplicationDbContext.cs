using Microsoft.EntityFrameworkCore;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TestValue> TestValues { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
