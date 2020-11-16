﻿using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.Articles.Queries.ListArticle
{
    public class ListArticleQuery : IRequest<PagedResponse<List<ArticleDto>>> 
    {
        public PaginationFilter Filter { get; set; }

        public ListArticleQuery(PaginationFilter filter)
        {
            Filter = filter;
        }

    }
}