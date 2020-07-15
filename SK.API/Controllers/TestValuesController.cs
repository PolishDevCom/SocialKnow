using MediatR;
using Microsoft.AspNetCore.Mvc;
using SK.Application.TestValues.Commands.Create;
using SK.Application.TestValues.Commands.Delete;
using SK.Application.TestValues.Commands.Edit;
using SK.Application.TestValues.Queries;
using SK.Application.TestValues.Queries.Details;
using SK.Application.TestValues.Queries.List;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class TestValuesController : ApiController
    {
        private readonly IMediator _mediator;
        public TestValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestValueDto>>> List()
        {
            return await _mediator.Send(new ListTestValueQuery.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestValueDto>> Details(int id)
        {
            return await _mediator.Send(new DetailsTestValueQuery.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTestValueCommand.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, EditTestValueCommand.Command command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTestValueCommand.Command { Id = id });

            return NoContent();
        }
    }
}
