using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Response;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Queries
{
    public class GetAvailableRooms : IRequest<Result<List<GetRoomsResponseDto>>>
    {

    }
    public class GetAvailableRoomsHandler : IRequestHandler<GetAvailableRooms, Result<List<GetRoomsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAvailableRoomsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<GetRoomsResponseDto>>> Handle(GetAvailableRooms request, CancellationToken cancellationToken)
        {
            var availableRooms = await _unitOfWork.roomRepository.GetWhereAsync(
             predicate: r => r.Status == "Available");
            if (availableRooms == null)
            {
                return Result<List<GetRoomsResponseDto>>.ErrorResult("All rooms are Unavailable.", HttpStatusCode.NotFound);
            }
            var roomDto = availableRooms.Select(room => new GetRoomsResponseDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                Status = room.Status,
                DateCreated = room.DateCreated,
                RoomTypeId = room.RoomTypeId,
                RoomTypeName = room.RoomType.TypeName, // Add RoomTypeName details
                RoomAmenitiesId = room.RoomAmenitiesId,
                Urls = room.Url
            }).ToList();
            return Result<List<GetRoomsResponseDto>>.SuccessResult(roomDto, HttpStatusCode.OK);
        }

    }

}
