using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Dtos.Request;
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
        [HttpPost("create-amenity")]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            var result = await _mediator.Send(new DeleteAmenity(id));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("GetAmenityByRoomAmenities/{roomAmenitiesId}")]
        public async Task<IActionResult> GetAmenitiesByRoomAmenitiesId(int roomAmenitiesId)
        {

            var result = await _mediator.Send(new GetAmenitiesByRoomAmenitiesId(roomAmenitiesId));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);

        }

        [HttpPut("activate-deactivate")]
        public async Task<IActionResult> ActivateDeactivateAmenity([FromBody] ActivateDeactivateAmenityRequestDto requestDto)
        {

            var command = new ActivateDeactivateAmenity(requestDto.AmenityId, requestDto.IsActive);
            var result = await _mediator.Send(command);

            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<Result<AssignAmenityToRoomAmenityResponseDto>>> AssignAmenityToRoomAmenity(AssignAmenityToRoomAmenityRequestDto request)
        {
            var command = new AssignAmenityToRoomAmenity(request.AmenityId, request.RoomAmenitiesId);
            var result = await _mediator.Send(command);

            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
    }
}

