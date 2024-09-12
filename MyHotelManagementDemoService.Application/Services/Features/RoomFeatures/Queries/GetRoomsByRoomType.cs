using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GetRoomsByRoomTypeHandler> _logger;

        public GetRoomsByRoomTypeHandler(IUnitOfWork unitOfWork, ILogger<GetRoomsByRoomTypeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<List<GetRoomsByRoomTypeResponseDto>>> Handle(GetRoomsByRoomType request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _unitOfWork.roomRepository.GetWhereAndIncludeAsync(
                    r => r.RoomTypeId == request.RoomTypeId,
                    include: r => r.Include(rt => rt.RoomType)
                );

                if (rooms == null || !rooms.Any())
                {
                    _logger.LogWarning("No rooms found for room type ID: {RoomTypeId}", request.RoomTypeId);
                    return Result<List<GetRoomsByRoomTypeResponseDto>>.NotFound("No rooms found");
                }

                var roomDtos = rooms.Select(r => new GetRoomsByRoomTypeResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Price = r.Price,
                    RoomTypeName = r.RoomType?.TypeName
                }).ToList();

                _logger.LogInformation("Rooms retrieved successfully for room type ID: {RoomTypeId}", request.RoomTypeId);

                return Result<List<GetRoomsByRoomTypeResponseDto>>.SuccessResult(roomDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rooms for room type ID: {RoomTypeId}", request.RoomTypeId);
                return Result<List<GetRoomsByRoomTypeResponseDto>>.InternalServerError();
            }
        }
    }

    public class GetRoomsByRoomTypeResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string RoomTypeName { get; set; }
    }
}

