using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
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
        private readonly ILogger<GetRoomByIdHandler> _logger;

        public GetRoomByIdHandler(IUnitOfWork unitOfWork, ILogger<GetRoomByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<GetRoomByIdResponseDto>> Handle(GetRoomById request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _unitOfWork.roomRepository.GetWhereAndIncludeAsync(
                    x => x.Id == request.Id,
                    include: x => x.Include(y => y.RoomType).Include(y => y.RoomAmenities).ThenInclude(z => z.Amenities)
                );

                if (room == null || !room.Any())
                {
                    _logger.LogWarning("Room: {Id} not found", request.Id);
                    return Result<GetRoomByIdResponseDto>.NotFound("Room not found");
                }

                var roomDto = room.Select(r => new GetRoomByIdResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Price = r.Price,
                    Status = r.Status,
                    DateCreated = r.DateCreated,
                    RoomType = new RoomTypeResponseDto
                    {
                        Id = r.RoomType.Id,
                        TypeName = r.RoomType.TypeName,
                        Description = r.RoomType.Description,
                        AccessibilityFeatures = r.RoomType.AccessibilityFeatures,
                    },
                    RoomAmenities = new RoomAmenitiesResponseDto
                    {
                        Id = r.RoomAmenities.Id,
                        Name = r.RoomAmenities.Name,
                        Amenities = r.RoomAmenities.Amenities.Select(a => a.Name).ToList(),
                    },
                    Urls = r.Url,
                }).FirstOrDefault();

                _logger.LogInformation("Room: {Id} retrieved successfully", request.Id);

                return Result<GetRoomByIdResponseDto>.SuccessResult(roomDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving room: {Id}", request.Id);
                return Result<GetRoomByIdResponseDto>.InternalServerError();
            }
        }
    }

    public class GetRoomByIdResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public RoomTypeResponseDto RoomType { get; set; }
        public RoomAmenitiesResponseDto RoomAmenities { get; set; }
        public List<string> Urls { get; set; }
    }

    public class RoomTypeResponseDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string AccessibilityFeatures { get; set; }
    }

    public class RoomAmenitiesResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Amenities { get; set; }
    }
}
