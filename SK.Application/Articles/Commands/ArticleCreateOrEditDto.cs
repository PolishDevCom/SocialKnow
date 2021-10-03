using System;

namespace SK.Application.Articles.Commands
{
    public class ArticleCreateOrEditDto
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
    }
}