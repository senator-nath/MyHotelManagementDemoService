using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command;
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
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequestDto requestDto)
        {
            var room = await _mediator.Send(new CreateRoom(requestDto));
            return StatusCode((int)room.statusCode, room.Success ? room.Data : room.Message);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomRequestDto requestDto)
        {
            var result = await _mediator.Send(new UpdateRoom(id, requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Message : result.Message);
        }



    }
}

