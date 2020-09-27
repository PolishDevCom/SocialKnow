using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Queries;
using SK.Application.Articles.Queries.DetailsArticle;
using SK.Application.Articles.Queries.ListArticle;
using SK.Application.TestValues.Commands.DeleteTestValue;
using SK.Application.TestValues.Commands.EditTestValue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class ArticlesController : ApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ArticleDto>>> List()
        {
            return await Mediator.Send(new ListArticleQuery());
        }

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
            await Mediator.Send(new EditArticleCommand(request.Id, request.Name));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteArticleCommand { Id = id });

            return NoContent();
        }
    }
}
