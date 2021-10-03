using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Common.Interfaces
{
    public interface IPaginationService<TEntity, TDto>
         where TEntity : AuditableEntity
         where TDto : class
    {
        Task<PagedResponse<List<TDto>>> GetPagedData(PaginationFilter validFilter, string route, CancellationToken cancellationToken);

        Task<PagedResponse<List<TDto>>> GetPagedData<TKey>(PaginationFilter validFilter, string route, CancellationToken cancellationToken,
            Expression<Func<TEntity, TKey>> additionalOrder, bool descending = true);
    }
}