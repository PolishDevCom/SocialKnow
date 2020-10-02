using MediatR;
using System;

namespace SK.Application.Articles.Queries.DetailsArticle
{
    public class DetailsArticleQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }
    }
}
