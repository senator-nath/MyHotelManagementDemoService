using BlogApp.Application.Helpers;
using MediatR;
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
    public class GetAllRooms : IRequest<Result<List<GetRoomsResponseDto>>>
    {
    }
    public class GetAllRoomsHandler : IRequestHandler<GetAllRooms, Result<List<GetRoomsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllRoomsHandler> _logger;

        public GetAllRoomsHandler(IUnitOfWork unitOfWork, ILogger<GetAllRoomsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<List<GetRoomsResponseDto>>> Handle(GetAllRooms request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _unitOfWork.roomRepository.GetAllAsync();

                if (rooms == null || !rooms.Any())
                {
                    _logger.LogWarning("No rooms found");
                    return Result<List<GetRoomsResponseDto>>.ErrorResult("No rooms found", HttpStatusCode.NotFound);
                }

                var roomDtos = rooms.Select(room => new GetRoomsResponseDto
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    Price = room.Price,
                    Status = room.Status,
                    DateCreated = room.DateCreated,
                    RoomTypeId = room.RoomTypeId,
                    RoomAmenitiesId = room.RoomAmenitiesId,
                    Urls = room.Url
                }).ToList();

                _logger.LogInformation("Rooms retrieved successfully");

                return Result<List<GetRoomsResponseDto>>.SuccessResult(roomDtos, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rooms");
                return Result<List<GetRoomsResponseDto>>.ErrorResult("Error retrieving rooms", HttpStatusCode.InternalServerError);
            }
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
        public string RoomTypeName { get; set; }
        public int RoomAmenitiesId { get; set; }
        public string RoomAmenitiesName { get; set; }
        public List<string> Urls { get; set; }
    }
}