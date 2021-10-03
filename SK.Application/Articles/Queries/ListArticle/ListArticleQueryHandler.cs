using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Queries.ListArticle
{
    public class ListArticleQueryHandler : IRequestHandler<ListArticleQuery, PagedResponse<List<ArticleDto>>>
    {
        private readonly IPaginationService<Article, ArticleDto> _paginationService;

        public ListArticleQueryHandler(IPaginationService<Article, ArticleDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<ArticleDto>>> Handle(ListArticleQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}