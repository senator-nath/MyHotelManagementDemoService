using HotelManagement.Application.Command.RoomType;
using HotelManagement.Application.Query.RoomType;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-room-type")]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomTypeRequestDto requestDto)
        {
            var roomType = await _mediator.Send(new CreateRoomTypeCommand(requestDto));
            return StatusCode((int)roomType.statusCode, roomType.Success ? roomType.Data : roomType.Message);
        }
        [HttpDelete("delete-room-type/{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var result = await _mediator.Send(new DeleteRoomTypeCommand(id));
            return StatusCode((int)result.statusCode, result.Success ? result.Message : result.Message);
        }
        [HttpPut("update-room-type")]
        public async Task<IActionResult> UpdateRoomType([FromBody] RoomTypeRequestDto requestDto)
        {
            var roomType = await _mediator.Send(new UpdateRoomTypeCommand(requestDto));
            return StatusCode((int)roomType.statusCode, roomType.Success ? roomType.Data : roomType.Message);
        }
        [HttpGet("get-all-room-types")]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var roomTypes = await _mediator.Send(new GetAllRoomTypeQuery());
            return StatusCode((int)roomTypes.statusCode, roomTypes.Success ? roomTypes.Data : roomTypes.Message);
        }
        [HttpGet("get-room-type-by-id/{id}")]
        public async Task<IActionResult> GetRoomTypeById(int id)
        {
            var roomType = await _mediator.Send(new GetRoomTypeByIdQuery(id));
            return StatusCode((int)roomType.statusCode, roomType.Success ? roomType.Data : roomType.Message);
        }
    }
}
