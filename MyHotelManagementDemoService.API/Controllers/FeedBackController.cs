using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Services.Features.FeedBackFeatures.Command;
using MyHotelManagementDemoService.Application.Services.Features.FeedBackFeatures.Queries;
using MyHotelManagementDemoService.Domain.Entities;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeedBackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequestDto requestDto)
        {
            var result = await _mediator.Send(new CreateFeedback(requestDto));
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _mediator.Send(new GetFeedbacks());
            return StatusCode((int)result.statusCode, result.Success ? result.Data : result.Message);
        }
    }
}
