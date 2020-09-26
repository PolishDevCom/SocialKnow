using MediatR;

namespace SK.Application.TestValues.Queries.DetailsTestValue
{
    public class DetailsArticleQuery : IRequest<ArticleDto>
    {
        public int Id { get; set; }
    }
}
