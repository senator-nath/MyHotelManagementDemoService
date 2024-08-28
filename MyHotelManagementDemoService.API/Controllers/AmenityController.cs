using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command;
using System.Threading.Tasks;

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
    }
}
