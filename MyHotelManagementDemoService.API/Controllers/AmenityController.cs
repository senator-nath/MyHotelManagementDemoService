﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command;
using MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Queries;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command;
using MyHotelManagementDemoService.Domain.Entities;
using System.Threading.Tasks;
using static MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command.UpdateAmenityHandler;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AmenityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityRequestDto requestDto)
        {
            var room = await _mediator.Send(new CreateAmenity(requestDto));
            return StatusCode((int)room.statusCode, room.Success ? room.Data : room.Message);
        }
        [HttpPut("Update-Amenity/{id}")]
        public async Task<IActionResult> UpdateAmenity(int id, [FromBody] UpdateAmenityRequestDto requestDto)
        {
            var result = await _mediator.Send(new UpdateAmenity(id, requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("Get-All-Amenities")]
        public async Task<IActionResult> GetAllAmenities()
        {
            var result = await _mediator.Send(new GetAmenities());

            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
    }
}
