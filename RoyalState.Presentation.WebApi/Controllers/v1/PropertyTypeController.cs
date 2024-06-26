﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RoyalState.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes;
using RoyalState.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Property type management")]
    public class PropertyTypeController : BaseApiController
    {

        [HttpGet]
        [SwaggerOperation(
           Summary = "Property type list",
           Description = "Returns a list of all the Property types"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var propertyTypes = await Mediator.Send(new GetAllPropertyTypeQuery());

            if (propertyTypes.Data == null || propertyTypes.Data.Count == 0)
            {
                return NoContent();
            }

            if (!propertyTypes.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(propertyTypes);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Property type by id",
           Description = "Returns a property type using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {

            var propertyType = await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id });

            if (propertyType.Data == null)
            {
                return NoContent();
            }

            if (!propertyType.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(propertyType);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
         Summary = "Property type creation",
         Description = "Recieves the parameters for creating a property type"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreatePropertyTypeCommand command)
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
             Summary = "Property type update",
             Description = "Recieves the parameters for modifying a property type"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdatePropertyTypeCommand command)
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
            Summary = "Property type removal",
            Description = "Recieves parameters for eliminating a property type"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeletePropertyTypeByIdCommand { Id = id });
            return NoContent();
        }

    }
}
