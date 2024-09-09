using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using MediatR;
using BlogApp.Application.Helpers;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Query.Booking
{
    public class CheckBookingAvailabilityQuery : IRequest<Result<List<AvailableRoomsResponse>>>
    {


        public List<AvailableRooms> Request { get; set; }
        public CheckBookingAvailabilityQuery(List<AvailableRooms> request)
        {
            Request = request;
        }
    }

    public class AvailableRooms
    {
        public int RoomId { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
    public class AvailableRoomsResponse
    {
        public int RoomNumber { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public string Message { get; set; }
    }

    public class CheckRoomAvailabilityQueryHandler : IRequestHandler<CheckBookingAvailabilityQuery, Result<List<AvailableRoomsResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckRoomAvailabilityQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AvailableRoomsResponse>>> Handle(CheckBookingAvailabilityQuery request,
        CancellationToken cancellationToken)
        {
            List<AvailableRoomsResponse> resp = new List<AvailableRoomsResponse>();
            foreach (var req in request.Request)
            {
                var conflictingBookings = await _unitOfWork.bookingRepository.GetWhereAsync(b =>
                b.RoomId == req.RoomId &&
                b.CheckInDate < req.CheckOutDate &&
                b.CheckOutDate > req.CheckinDate);
                if (conflictingBookings.Any())
                {
                    resp.Add(new AvailableRoomsResponse
                    {
                        RoomNumber = 1,
                        CheckinDate = req.CheckinDate,
                        CheckOutDate = req.CheckOutDate,
                        Message = "Room is not available."
                    });
                }
                else
                {
                    resp.Add(new AvailableRoomsResponse
                    {
                        RoomNumber = 1,
                        CheckinDate = req.CheckinDate,
                        CheckOutDate = req.CheckOutDate,
                        Message = "Room is available."
                    });
                }
            }
            return Result<List<AvailableRoomsResponse>>.SuccessResult(resp);

            //return !conflictingBookings.Any(); // returns true if no no conflicting booking
        }
    }

}