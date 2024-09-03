using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Queries
{
    public class GetRoomById : IRequest<Result<GetRoomByIdResponseDto>>
    {
        public int Id { get; }

        public GetRoomById(int id)
        {
            Id = id;
        }
    }
    public class GetRoomByIdHandler : IRequestHandler<GetRoomById, Result<GetRoomByIdResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoomByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetRoomByIdResponseDto>> Handle(GetRoomById request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.roomRepository.GetByIdAsync(request.Id);

            if (room == null)
            {
                return Result<GetRoomByIdResponseDto>.ErrorResult("Room not found", HttpStatusCode.NotFound); ;
            }

            var roomDto = new GetRoomByIdResponseDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                Status = room.Status,
                DateCreated = room.DateCreated,
                RoomTypeId = room.RoomTypeId,
                RoomAmenitiesId = room.RoomAmenitiesId,
                Urls = room.Url
            };

            return Result<GetRoomByIdResponseDto>.SuccessResult(roomDto, HttpStatusCode.OK);
        }
    }
    public class GetRoomByIdResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
        public List<string> Urls { get; set; }
    }
}
