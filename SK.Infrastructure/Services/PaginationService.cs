using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Infrastructure.Services
{
    public class PaginationService<TEntity, TDto> : IPaginationService<TEntity, TDto>
         where TEntity : AuditableEntity
         where TDto : class
    {
        private readonly IApplicationDbContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public PaginationService(IApplicationDbContext context, IUriService uriService, IMapper mapper)
        {
            _context = context;
            _uriService = uriService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<TDto>>> GetPagedData(PaginationFilter validFilter, string route, CancellationToken cancellationToken)
        {
            var pagedData = await _context.DbSet<TEntity>()
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .OrderByDescending(a => a.Created)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var totalRecords = await _context.DbSet<TEntity>().CountAsync();
            return PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
        }

        public async Task<PagedResponse<List<TDto>>> GetPagedData<TKey>(PaginationFilter validFilter, string route, CancellationToken cancellationToken,
            Expression<Func<TEntity, TKey>> additionalOrder, bool descending = true)
        {
            IOrderedQueryable<TEntity> orderedQueryable;

            if (descending)
            {
                orderedQueryable = _context.DbSet<TEntity>().OrderByDescending(additionalOrder);
            }
            else
            {
                orderedQueryable = _context.DbSet<TEntity>().OrderBy(additionalOrder);              
            }

            var pagedData = await orderedQueryable
                .ThenByDescending(a => a.Created)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var totalRecords = await _context.DbSet<TEntity>().CountAsync();
            return PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
        }
    }
}
