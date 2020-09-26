using MediatR;
using System.Collections.Generic;

namespace SK.Application.TestValues.Queries.ListTestValue
{
    public class ListArticleQuery : IRequest<List<ArticleDto>> { }
}