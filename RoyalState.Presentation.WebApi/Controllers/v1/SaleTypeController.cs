﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RoyalState.Core.Application.Features.SaleTypes.Commands.DeleteSaleType;
using RoyalState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RoyalState.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes;
using RoyalState.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Sale type management")]
    public class SaleTypeController : BaseApiController
    {

        [HttpGet]
        [SwaggerOperation(
           Summary = "Sale type list",
           Description = "Returns a list of all the sale types"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            var saleTypes = await Mediator.Send(new GetAllSaleTypesQuery());

            if (saleTypes.Data == null || saleTypes.Data.Count == 0)
            {
                return NoContent();
            }

            if (!saleTypes.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(saleTypes);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Sale type by id",
           Description = "Returns a sale type using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {

            var saleType = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });

            if (saleType.Data == null)
            {
                return NoContent();
            }

            if (!saleType.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(saleType);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
         Summary = "Sale type creation",
         Description = "Recieves the parameters for creating a sale type"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateSaleTypeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return Created(string.Empty, null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(
             Summary = "Sale type update",
             Description = "Recieves the parameters for modifying a sale type"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateSaleTypeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Sale type removal",
            Description = "Recieves parameters for eliminating a sale type"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });
            return NoContent();
        }

    }
}
