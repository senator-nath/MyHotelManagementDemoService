using BlogApp.Application.Helpers;
using HotelManagement.Application.Query.Booking;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHotelManagementDemoService.Application.Services.Features.BookingFeatures.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-booking")]
        public async Task<ActionResult<Result<BookingResponse>>> CreateBooking(BookingRequestDto request)
        {
            var command = new CreateBookingCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        [Route("CheckBookingAvailability")]
        public async Task<IActionResult> CheckBookingAvailability(List<AvailableRooms> availableRooms)
        {
            var query = new CheckBookingAvailabilityQuery(availableRooms);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
