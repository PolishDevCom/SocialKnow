using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Articles.Commands;
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
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns>List of articles</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<ArticleDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListArticleQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Fetches a single article by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Article ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> Details(Guid id)
        {
            return Ok(await Mediator.Send(new DetailsArticleQuery(id)));
        }

        /// <summary>
        /// Adds a new article.
        /// </summary>
        /// <param name="newArticle">New article</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ArticleCreateOrEditDto newArticle)
        {
            return Ok(await Mediator.Send(new CreateArticleCommand(newArticle)));
        }

        /// <summary>
        /// Updates an existing article selected by id.
        /// </summary>
        /// <param name="editArticle">Edited article</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] ArticleCreateOrEditDto editArticle)
        {
            await Mediator.Send(new EditArticleCommand(editArticle));
            return NoContent();
        }

        /// <summary>
        /// Deletes an article with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Article ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteArticleCommand(id));
            return NoContent();
        }
    }
}
