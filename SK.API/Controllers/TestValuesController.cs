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
    public class TestValuesController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<TestValueDto>>> List()
        {
            return await Mediator.Send(new ListTestValueQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestValueDto>> Details(int id)
        {
            return await Mediator.Send(new DetailsTestValueQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTestValueCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] TestValueDto testValue)
        {
            await Mediator.Send(new EditTestValueCommand { Id = testValue.Id, Name = testValue.Name});

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTestValueCommand { Id = id });

            return NoContent();
        }
    }
}
