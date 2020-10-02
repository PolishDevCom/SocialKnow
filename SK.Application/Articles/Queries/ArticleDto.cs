using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Articles.Queries
{
    public class ArticleDto : IMapFrom<Article>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}
