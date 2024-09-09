using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{
    public class ActivateDeactivateAmenity : IRequest<Result<ActivateDeactivateAmenityRequestDto>>
    {
        public int AmenityId { get; }
        public bool IsActive { get; }

        public ActivateDeactivateAmenity(int amenityId, bool isActive)
        {
            AmenityId = amenityId;
            IsActive = isActive;
        }
    }

    public class ActivateDeactivateAmenityHandler : IRequestHandler<ActivateDeactivateAmenity, Result<ActivateDeactivateAmenityRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActivateDeactivateAmenityHandler> _logger;

        public ActivateDeactivateAmenityHandler(IUnitOfWork unitOfWork, ILogger<ActivateDeactivateAmenityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ActivateDeactivateAmenityRequestDto>> Handle(ActivateDeactivateAmenity request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Activating/Deactivating amenity with ID: {AmenityId}", request.AmenityId);

                if (request.AmenityId <= 0)
                {
                    _logger.LogWarning("Invalid amenity ID: {AmenityId}", request.AmenityId);
                    return Result<ActivateDeactivateAmenityRequestDto>.BadRequest();
                }

                var amenity = await _unitOfWork.amenityRepository.GetByIdAsync(request.AmenityId);

                if (amenity == null)
                {
                    _logger.LogWarning("Amenity not found: {AmenityId}", request.AmenityId);
                    return Result<ActivateDeactivateAmenityRequestDto>.NotFound("Amenity not found");
                }

                amenity.IsActive = request.IsActive;

                _unitOfWork.amenityRepository.Update(amenity);
                await _unitOfWork.Save();

                var responseDto = new ActivateDeactivateAmenityRequestDto
                {
                    AmenityId = amenity.Id,
                    IsActive = amenity.IsActive
                };

                _logger.LogInformation("Amenity with ID: {AmenityId} activated/deactivated successfully", request.AmenityId);

                return Result<ActivateDeactivateAmenityRequestDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating amenity with ID: {AmenityId}", request.AmenityId);
                return Result<ActivateDeactivateAmenityRequestDto>.InternalServerError();
            }
        }
    }
    public class ActivateDeactivateAmenityRequestDto
    {
        public int AmenityId { get; set; }
        public bool IsActive { get; set; }
    }
}

