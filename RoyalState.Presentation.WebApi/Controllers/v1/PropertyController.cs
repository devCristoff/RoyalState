﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Features.Properties.Queries.GetAllProperties;
using RoyalState.Core.Application.Features.Properties.Queries.GetPropertyByCode;
using RoyalState.Core.Application.Features.Properties.Queries.GetPropertyById;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Property management")]
    public class PropertyController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
          Summary = "Property List",
          Description = "Returns a list of all the properties"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var properties = await Mediator.Send(new GetAllPropertiesQuery());

            if (properties.Data == null || properties.Data.Count == 0)
            {
                return NoContent();
            }

            if (!properties.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(properties);

        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Property by id",
           Description = "Returns a property using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {

            if (id <= 0)
            {
                return NoContent();
            }

            var property = await Mediator.Send(new GetPropertyByIdQuery { Id = id });

            if (property.Data == null)
            {
                return NoContent();
            }

            if (!property.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }




            return Ok(property);
        }




        [HttpGet("Code/{code}")]
        [SwaggerOperation(
           Summary = "Property by code",
           Description = "Returns a property using the code as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode([FromQuery] GetPropertyByCodeParameter filter)
        {

            var property = await Mediator.Send(new GetPropertyByCodeQuery() { Code = filter.Code });
            if (property.Data == null)
            {
                return NoContent();
            }

            if (!property.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(property);
        }

    }
}
