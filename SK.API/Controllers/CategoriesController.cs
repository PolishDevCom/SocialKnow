using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Categories.Commands.DeleteCategory;
using SK.Application.Categories.Commands.EditCategory;
using SK.Application.Categories.Queries;
using SK.Application.Categories.Queries.ListCategory;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
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
        /// Fetches lists of categories with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns>List of categories</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<CategoryDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListCategoryQuery(paginationFilter, Request.Path.Value)));
        }

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

        /// <summary>
        /// Updates an existing category selected by id.
        /// </summary>
        /// <param name="editCategory">Edited category</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] CategoryCreateOrEditDto editCategory)
        {
            await Mediator.Send(new EditCategoryCommand(editCategory));
            return NoContent();
        }

        /// <summary>
        /// Deletes a category with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Category ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}
