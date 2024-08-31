using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public ActivateDeactivateAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ActivateDeactivateAmenityRequestDto>> Handle(ActivateDeactivateAmenity request, CancellationToken cancellationToken)
        {
            // Retrieve the amenity by Id
            var amenity = await _unitOfWork.amenityRepository.GetByIdAsync(request.AmenityId);

            if (amenity == null)
            {
                // Amenity with the specified Id does not exist
                return Result<ActivateDeactivateAmenityRequestDto>.ErrorResult("Amenity not found.", HttpStatusCode.NotFound);
            }

            // Update the IsActive status
            amenity.IsActive = request.IsActive;

            // Update the amenity in the repository
            _unitOfWork.amenityRepository.Update(amenity);
            await _unitOfWork.Save();

            // Prepare the response DTO
            var responseDto = new ActivateDeactivateAmenityRequestDto
            {
                AmenityId = amenity.Id, // Include the AmenityId in the response DTO
                IsActive = amenity.IsActive
            };

            // Return success result with response DTO
            return Result<ActivateDeactivateAmenityRequestDto>.SuccessResult(responseDto, HttpStatusCode.OK);
        }
    }

}

