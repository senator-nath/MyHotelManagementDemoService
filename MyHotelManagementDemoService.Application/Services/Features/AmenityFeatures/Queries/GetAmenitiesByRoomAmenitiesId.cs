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
using static MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Queries.GetAmenitiesByRoomAmenitiesIdHandler;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Queries
{
    public class GetAmenitiesByRoomAmenitiesId : IRequest<Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>>
    {
        public int RoomAmenitiesId { get; }

        public GetAmenitiesByRoomAmenitiesId(int roomAmenitiesId)
        {
            RoomAmenitiesId = roomAmenitiesId;
        }
    }


    public class GetAmenitiesByRoomAmenitiesIdHandler : IRequestHandler<GetAmenitiesByRoomAmenitiesId, Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAmenitiesByRoomAmenitiesIdHandler> _logger;

        public GetAmenitiesByRoomAmenitiesIdHandler(IUnitOfWork unitOfWork, ILogger<GetAmenitiesByRoomAmenitiesIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>> Handle(GetAmenitiesByRoomAmenitiesId request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting amenities by RoomAmenitiesId");

                var roomAmenities = await _unitOfWork.roomAmenitiesRepository.GetWhereAndIncludeAsync(
                    ra => ra.Id == request.RoomAmenitiesId,
                    include: ra => ra.Include(ra => ra.Amenities)
                );

                var roomAmenitiesEntity = roomAmenities.FirstOrDefault();
                if (roomAmenitiesEntity == null || !roomAmenitiesEntity.Amenities.Any())
                {
                    _logger.LogError("No amenities found for the specified RoomAmenitiesId");
                    return Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>.NotFound("No amenities found for the specified RoomAmenitiesId.");
                }

                var amenityDtos = roomAmenitiesEntity.Amenities.Select(amenity => new GetAmenitiesByRoomAmenitiesIdResponseDto
                {
                    Id = amenity.Id,
                    Name = amenity.Name,
                    Description = amenity.Description,
                    IsActive = amenity.IsActive
                }).ToList();

                _logger.LogInformation("Amenities retrieved successfully");

                return Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>.SuccessResult(amenityDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting amenities by RoomAmenitiesId");
                return Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>.InternalServerError();
            }
        }
    }

}

