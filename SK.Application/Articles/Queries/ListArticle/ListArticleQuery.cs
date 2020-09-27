using MediatR;
using System.Collections.Generic;

namespace SK.Application.Articles.Queries.ListArticle
{
    public class ListArticleQuery : IRequest<List<ArticleDto>> { }
}