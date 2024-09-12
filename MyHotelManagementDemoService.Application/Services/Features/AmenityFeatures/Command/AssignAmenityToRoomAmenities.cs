using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{

    public class AssignAmenityToRoomAmenity : IRequest<Result<AssignAmenityToRoomAmenityResponseDto>>
    {
        public int AmenityId { get; }
        public int RoomAmenitiesId { get; }

        public AssignAmenityToRoomAmenity(int amenityId, int roomAmenitiesId)
        {
            AmenityId = amenityId;
            RoomAmenitiesId = roomAmenitiesId;
        }
    }

    public class AssignAmenityToRoomAmenityHandler : IRequestHandler<AssignAmenityToRoomAmenity, Result<AssignAmenityToRoomAmenityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignAmenityToRoomAmenityHandler> _logger;

        public AssignAmenityToRoomAmenityHandler(IUnitOfWork unitOfWork, ILogger<AssignAmenityToRoomAmenityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<AssignAmenityToRoomAmenityResponseDto>> Handle(AssignAmenityToRoomAmenity request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Assigning amenity to room amenity");

                var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.AmenityId);

                if (amenityEntity == null)
                {
                    _logger.LogError("Amenity not found");
                    return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Amenity not found");
                }

                if (!amenityEntity.IsActive)
                {
                    _logger.LogError("Amenity is not active");
                    return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Amenity is not active");
                }

                var roomAmenityEntity = await _unitOfWork.roomAmenitiesRepository.GetByColumnAsync(a => a.Id == request.RoomAmenitiesId);

                if (roomAmenityEntity == null)
                {
                    _logger.LogError("Room amenity not found");
                    return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Room amenity not found");
                }

                roomAmenityEntity.Amenities.Add(amenityEntity);
                _unitOfWork.roomAmenitiesRepository.Update(roomAmenityEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Amenity assigned to room amenity successfully");

                var responseDto = new AssignAmenityToRoomAmenityResponseDto
                {
                    AmenityId = amenityEntity.Id,
                    RoomAmenitiesId = roomAmenityEntity.Id
                };

                return Result<AssignAmenityToRoomAmenityResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning amenity to room amenity");
                return Result<AssignAmenityToRoomAmenityResponseDto>.InternalServerError();
            }
        }
    }

    public class AssignAmenityToRoomAmenityResponseDto
    {
        public int AmenityId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }

    public class AssignAmenityToRoomAmenityRequestDto
    {
        public int AmenityId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}
