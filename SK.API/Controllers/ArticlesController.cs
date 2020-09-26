using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.TestValues.Commands.CreateTestValue;
using SK.Application.TestValues.Commands.DeleteTestValue;
using SK.Application.TestValues.Commands.EditTestValue;
using SK.Application.TestValues.Queries;
using SK.Application.TestValues.Queries.DetailsTestValue;
using SK.Application.TestValues.Queries.ListTestValue;
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
        public async Task<ActionResult<ArticleDto>> Details(int id)
        {
            return await Mediator.Send(new DetailsArticleQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] ArticleDto request)
        {
            return await Mediator.Send(new CreateArticleCommand(request.Id, request.Name));
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
