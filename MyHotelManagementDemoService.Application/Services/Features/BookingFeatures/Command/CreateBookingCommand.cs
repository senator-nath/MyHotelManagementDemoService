using System;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using FluentValidation;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Domain.Entities;

namespace MyHotelManagementDemoService.Application.Services.Features.BookingFeatures.Command
{
    public class CreateBookingCommand : IRequest<Result<BookingResponse>>
    {
        internal readonly BookingRequestDto requestDto;

        public CreateBookingCommand(BookingRequestDto requestDto)
        {
            this.requestDto = requestDto;
        }
    }

    public class BookingValidator : AbstractValidator<Domain.Entities.Booking>
    {
        public BookingValidator()
        {
            RuleFor(x => x.BookingDate)
                .NotEmpty()
                .WithMessage("Booking date is required.")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Booking date cannot be in the future.");

            RuleFor(x => x.CheckInDate)
                .NotEmpty()
                .WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(x => x.BookingDate)
                .WithMessage("Check-in date must be on or after the booking date.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty()
                .WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("Check-out date must be after the check-in date.");

            RuleFor(x => x.NumberOfGuests)
                .GreaterThan(0)
                .WithMessage("Number of occupants must be greater than zero.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.RoomId)
                .GreaterThan(0)
                .WithMessage("Room ID must be a positive integer.");

            //RuleFor(x => x.PaymentId)
            //    .GreaterThan(0)
            //    .WithMessage("Payment ID must be a positive integer.");
        }
    }

    public class BookingResponse
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfOcupant { get; set; }

        public string UserId { get; set; }
        public int RoomId { get; set; }

        public int PaymentId { get; set; }

    }
    public class BookingRequestDto
    {
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfOcupant { get; set; }
        //public string Status { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }

        public int PaymentId { get; set; }
    }

    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Result<BookingResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.paymentRepository
            .GetByColumnAsync(x => x.Id == request.requestDto.PaymentId);

            if (payment == null)
            {
                return Result<BookingResponse>.NotFound("");
            }
            var booking = new Booking()
            {
                BookingDate = request.requestDto.BookingDate,
                CheckInDate = request.requestDto.CheckInDate,
                CheckOutDate = request.requestDto.CheckOutDate,
                NumberOfGuests = request.requestDto.NumberOfOcupant,
                UserId = request.requestDto.UserId,
                RoomId = request.requestDto.RoomId,
                //PaymentId = request.requestDto.PaymentId,
            };

            var response = new BookingResponse
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                BookingDate = booking.BookingDate,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                NumberOfOcupant = booking.NumberOfGuests,
            };

            return Result<BookingResponse>.SuccessResult(response);


        }
    }
}