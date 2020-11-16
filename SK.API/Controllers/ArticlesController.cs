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
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<ArticleDto>>>> List([FromQuery] PaginationFilter filter)
        {
            return await Mediator.Send(new ListArticleQuery(filter));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> Details(Guid id)
        {
            return await Mediator.Send(new DetailsArticleQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ArticleDto request)
        {
            return await Mediator.Send(new CreateArticleCommand(request));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] ArticleDto request)
        {
            await Mediator.Send(new EditArticleCommand(request));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteArticleCommand { Id = id });

            return NoContent();
        }
    }
}
