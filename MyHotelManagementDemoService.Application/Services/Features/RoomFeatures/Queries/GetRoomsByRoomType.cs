using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Queries
{
    public class GetRoomsByRoomType : IRequest<Result<List<GetRoomsByRoomTypeResponseDto>>>
    {
        public int RoomTypeId { get; }

        public GetRoomsByRoomType(int roomTypeId)
        {
            RoomTypeId = roomTypeId;
        }
    }
    public class GetRoomsByRoomTypeHandler : IRequestHandler<GetRoomsByRoomType, Result<List<GetRoomsByRoomTypeResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoomsByRoomTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetRoomsByRoomTypeResponseDto>>> Handle(GetRoomsByRoomType request, CancellationToken cancellationToken)
        {
            // Fetch rooms that belong to the specified RoomTypeId
            var rooms = await _unitOfWork.roomRepository.GetWhereAsync(r => r.RoomTypeId == request.RoomTypeId);

            if (rooms == null || !rooms.Any())
            {
                return Result<List<GetRoomsByRoomTypeResponseDto>>.ErrorResult("No rooms found for the specified RoomType.", HttpStatusCode.NotFound);
            }

            var roomDtos = rooms.Select(room => new GetRoomsByRoomTypeResponseDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                Status = room.Status,
                DateCreated = room.DateCreated,
                RoomTypeId = room.RoomTypeId,
                RoomTypeName = room.RoomType.TypeName, // Mapping RoomType name
                Url = room.Url
            }).ToList();

            return Result<List<GetRoomsByRoomTypeResponseDto>>.SuccessResult(roomDtos, HttpStatusCode.OK);
        }
    }
}

