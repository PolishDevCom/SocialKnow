using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Articles.Queries
{
    public class ArticleDto : IMapFrom<Article>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Article title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Image URI
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Article abstract
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Article content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Article author
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime Created { get; set; }
    }
}
