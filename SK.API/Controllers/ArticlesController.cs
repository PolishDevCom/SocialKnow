using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Commands.DeleteArticle;
using SK.Application.Articles.Commands.EditArticle;
using SK.Application.Articles.Queries;
using SK.Application.Articles.Queries.DetailsArticle;
using SK.Application.Articles.Queries.ListArticle;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class ArticlesController : ApiController
    {
        /// <summary>
        /// Fetches lists of articles with selected pagination filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<ArticleDto>>>> List([FromQuery] PaginationFilter filter)
        {
            return await Mediator.Send(new ListArticleQuery(filter, Request.Path.Value));
        }

        /// <summary>
        /// Fetches a single article by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> Details(Guid id)
        {
            return await Mediator.Send(new DetailsArticleQuery { Id = id });
        }

        /// <summary>
        /// Adds a new article.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ArticleDto request)
        {
            return await Mediator.Send(new CreateArticleCommand(request));
        }

        /// <summary>
        /// Updates an existing article selected by id.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] ArticleDto request)
        {
            await Mediator.Send(new EditArticleCommand(request));

            return NoContent();
        }

        /// <summary>
        /// Deletes an article with selected id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteArticleCommand { Id = id });

            return NoContent();
        }
    }
}
