using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.RefundFeatures.Command;
using MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefundController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateRefund")]
        public async Task<IActionResult> CreateRefund([FromBody] CreateRefundRequestDto requestDto)
        {
            var room = await _mediator.Send(new CreateRefund(requestDto));
            return StatusCode((int)room.statusCode, room.Success ? room.Data : room.Message);
        }
    }
}
