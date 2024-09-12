using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class GetAvailableRooms : IRequest<Result<List<GetAvailableRoomsResponseDto>>>
    {
    }

    public class GetAvailableRoomsHandler : IRequestHandler<GetAvailableRooms, Result<List<GetAvailableRoomsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAvailableRoomsHandler> _logger;

        public GetAvailableRoomsHandler(IUnitOfWork unitOfWork, ILogger<GetAvailableRoomsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<List<GetAvailableRoomsResponseDto>>> Handle(GetAvailableRooms request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _unitOfWork.roomRepository.GetWhereAndIncludeAsync(
                    r => r.Status == "Available",
                    include: r => r.Include(rt => rt.RoomType)
                );

                if (rooms == null || !rooms.Any())
                {
                    _logger.LogWarning("No available rooms found");
                    return Result<List<GetAvailableRoomsResponseDto>>.NotFound("No available rooms found");
                }

                var roomDtos = rooms.Select(r => new GetAvailableRoomsResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Price = r.Price,
                    RoomTypeName = r.RoomType?.TypeName
                }).ToList();

                _logger.LogInformation("Available rooms retrieved successfully");

                return Result<List<GetAvailableRoomsResponseDto>>.SuccessResult(roomDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available rooms");
                return Result<List<GetAvailableRoomsResponseDto>>.InternalServerError();
            }
        }
    }

    public class GetAvailableRoomsResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string RoomTypeName { get; set; }
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string RoomTypeName { get; set; }
    }

}
