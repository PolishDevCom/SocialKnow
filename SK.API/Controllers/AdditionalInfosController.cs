using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.AdditionalInfos.Commands;
using SK.Application.AdditionalInfos.Commands.CreateAdditionalInfo;
using SK.Application.AdditionalInfos.Commands.DeleteAdditionalInfo;
using SK.Application.AdditionalInfos.Commands.EditAdditionalInfo;
using SK.Application.AdditionalInfos.Queries;
using SK.Application.AdditionalInfos.Queries.ListAdditionalInfo;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdditionalInfosController : ApiController
    {
        /// <summary>
        /// Fetches lists of additional infos with selected pagination filter.
        /// </summary>
        /// <param name="paginationFilter">Pagination filter</param>
        /// <returns>List of additional infos</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<AdditionalInfoDto>>>> List([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await Mediator.Send(new ListAdditionalInfoQuery(paginationFilter, Request.Path.Value)));
        }

        /// <summary>
        /// Adds a new additional info.
        /// </summary>
        /// <param name="newAdditionalInfo">New additional info</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] AdditionalInfoCreateOrEditDto newAdditionalInfo)
        {
            return Ok(await Mediator.Send(new CreateAdditionalInfoCommand(newAdditionalInfo)));
        }

        /// <summary>
        /// Updates anadditional info selected by id.
        /// </summary>
        /// <param name="editAdditionalInfo">Edited additional info</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromBody] AdditionalInfoCreateOrEditDto editAdditionalInfo)
        {
            await Mediator.Send(new EditAdditionalInfoCommand(editAdditionalInfo));
            return NoContent();
        }

        /// <summary>
        /// Deletes an additional info with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Additional info ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteAdditionalInfoCommand(id));
            return NoContent();
        }
    }
}
