using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Queries;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequestDto requestDto)
        {
            var room = await _mediator.Send(new CreateRoom(requestDto));
            return StatusCode((int)room.statusCode, room.Success ? room.Data : room.Message);
        }
        [HttpPut("Update-room/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomRequestDto requestDto)
        {
            var result = await _mediator.Send(new UpdateRoom(id, requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("get-all-rooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await _mediator.Send(new GetAllRooms());
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpDelete("Delete-Room/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _mediator.Send(new DeleteRoom(id));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("Get-Rooms-By-Room-Type/{roomTypeId}")]
        public async Task<IActionResult> GetRoomsByRoomType(int roomTypeId)
        {
            var result = await _mediator.Send(new GetRoomsByRoomType(roomTypeId));

            return Ok(result.Data);
        }
        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeRoomStatus([FromBody] ChangeRoomStatusRequestDto requestDto)
        {
            var result = await _mediator.Send(new ChangeRoomStatus(requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var result = await _mediator.Send(new GetAvailableRooms());
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet("Get-Room-By-Id/{id}")]
        public async Task<ActionResult<Result<GetRoomByIdResponseDto>>> GetRoomById(int id)
        {
            var request = new GetRoomById(id);
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

