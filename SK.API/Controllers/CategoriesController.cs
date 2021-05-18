using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : ApiController
    {
        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="newCategory">New category</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CategoryCreateOrEditDto newCategory)
        {
            return Ok(await Mediator.Send(new CreateCategoryCommand(newCategory)));
        }
    }
}
