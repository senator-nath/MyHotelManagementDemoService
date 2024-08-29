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
    public class GetAllRooms : IRequest<Result<List<GetRoomsResponseDto>>>
    {
    }
    public class GetAllRoomsHandler : IRequestHandler<GetAllRooms, Result<List<GetRoomsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllRoomsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<GetRoomsResponseDto>>> Handle(GetAllRooms request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.roomRepository.GetAllAsync();
            var roomDto = room.Select(room => new GetRoomsResponseDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                Status = room.Status,
                DateCreated = room.DateCreated,
                RoomTypeId = room.RoomTypeId,
                //RoomTypeName = room.RoomType.Name,   
                RoomAmenitiesId = room.RoomAmenitiesId,
                //RoomAmenitiesName = room.RoomAmenities.Name,   
                Urls = room.Url
            }).ToList();
            return Result<List<GetRoomsResponseDto>>.SuccessResult(roomDto, HttpStatusCode.OK);
        }
    }
    public class GetRoomsResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }  // Added for RoomType details
        public int RoomAmenitiesId { get; set; }
        public string RoomAmenitiesName { get; set; }  // Added for RoomAmenities details
        public List<string> Urls { get; set; }
    }
}
