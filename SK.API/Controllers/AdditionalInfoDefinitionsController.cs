using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.AdditionalInfoDefinitions.Commands;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Commands.DeleteAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Queries;
using SK.Application.AdditionalInfoDefinitions.Queries.ListAdditionalInfoDefinition;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdditionalInfoDefinitionsController : ApiController
    {
        /// <summary>
        /// Fetches lists of additional info definitions with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns>List of additional info definition definitions</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<AdditionalInfoDefinitionDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListAdditionalInfoDefinitionQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Adds a new additional info definition.
        /// </summary>
        /// <param name="newAdditionalInfo">New additional info definition</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] AdditionalInfoDefinitionCreateOrEditDto newAdditionalInfo)
        {
            return Ok(await Mediator.Send(new CreateAdditionalInfoDefinitionCommand(newAdditionalInfo)));
        }

        /// <summary>
        /// Updates a additional info definition selected by id.
        /// </summary>
        /// <param name="editAdditionalInfo">Edited additional info definition</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] AdditionalInfoDefinitionCreateOrEditDto editAdditionalInfo)
        {
            await Mediator.Send(new EditAdditionalInfoDefinitionCommand(editAdditionalInfo));
            return NoContent();
        }

        /// <summary>
        /// Deletes a additional info definition with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Additional info definition ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteAdditionalInfoDefinitionCommand(id));
            return NoContent();
        }
    }
}
