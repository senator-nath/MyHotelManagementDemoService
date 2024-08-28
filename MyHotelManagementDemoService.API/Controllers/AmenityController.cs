using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command;
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
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateAmenityRequestDto requestDto)
        {
            var room = await _mediator.Send(new CreateAmenity(requestDto));
            return StatusCode((int)room.statusCode, room.Success ? room.Data : room.Message);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAmenity([FromBody] UpdateAmenityRequestDto requestDto)
        {
            if (!Request.Headers.TryGetValue("AmenityId", out var amenityIdValue) || !int.TryParse(amenityIdValue, out var amenityId))
            {
                return BadRequest("AmenityId header is missing or invalid.");
            }

            requestDto.Id = amenityId; // Set the Id from the header

            var result = await _mediator.Send(new UpdateAmenity(requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
    }
}

