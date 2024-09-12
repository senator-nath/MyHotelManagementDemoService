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

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Queries
{
    public class GetAmenities : IRequest<Result<List<GetAmenitiesResponseDto>>>
    {
    }
    public class GetAmenitiesHandler : IRequestHandler<GetAmenities, Result<List<GetAmenitiesResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAmenitiesHandler> _logger;

        public GetAmenitiesHandler(IUnitOfWork unitOfWork, ILogger<GetAmenitiesHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<List<GetAmenitiesResponseDto>>> Handle(GetAmenities request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting amenities");

                var amenities = await _unitOfWork.amenityRepository.GetAllAsync();

                var amenityDtos = amenities.Select(amenity => new GetAmenitiesResponseDto
                {
                    Id = amenity.Id,
                    Name = amenity.Name,
                    Description = amenity.Description,
                    IsActive = amenity.IsActive,
                    RoomAmenitiesId = amenity.RoomAmenitiesId
                }).ToList();

                _logger.LogInformation("Amenities retrieved successfully");

                return Result<List<GetAmenitiesResponseDto>>.SuccessResult(amenityDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting amenities");
                return Result<List<GetAmenitiesResponseDto>>.InternalServerError();
            }
        }
    }

}


