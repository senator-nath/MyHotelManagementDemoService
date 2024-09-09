using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public GetAmenitiesByRoomAmenitiesIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>> Handle(GetAmenitiesByRoomAmenitiesId request, CancellationToken cancellationToken)
        {
            // Fetch RoomAmenities including the related Amenities by RoomAmenitiesId
            var roomAmenities = await _unitOfWork.roomAmenitiesRepository.GetWhereAndIncludeAsync(
                ra => ra.Id == request.RoomAmenitiesId,
                include: ra => ra.Include(ra => ra.Amenities)
            );

            // Check if RoomAmenities were found
            var roomAmenitiesEntity = roomAmenities.FirstOrDefault();
            if (roomAmenitiesEntity == null || !roomAmenitiesEntity.Amenities.Any())
            {
                return Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>.NotFound("No amenities found for the specified RoomAmenitiesId.");
            }

            // Map amenities to DTOs
            var amenityDtos = roomAmenitiesEntity.Amenities.Select(amenity => new GetAmenitiesByRoomAmenitiesIdResponseDto
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsActive = amenity.IsActive
            }).ToList();

            return Result<List<GetAmenitiesByRoomAmenitiesIdResponseDto>>.SuccessResult(amenityDtos);
        }
    }

}

