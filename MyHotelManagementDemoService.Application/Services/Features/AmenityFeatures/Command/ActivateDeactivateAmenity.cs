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

            var amenity = await _unitOfWork.amenityRepository.GetByIdAsync(request.AmenityId);

            if (amenity == null)
            {

                return Result<ActivateDeactivateAmenityRequestDto>.ErrorResult("Amenity not found.", HttpStatusCode.NotFound);
            }


            amenity.IsActive = request.IsActive;


            _unitOfWork.amenityRepository.Update(amenity);
            await _unitOfWork.Save();


            var responseDto = new ActivateDeactivateAmenityRequestDto
            {
                AmenityId = amenity.Id,
                IsActive = amenity.IsActive
            };


            return Result<ActivateDeactivateAmenityRequestDto>.SuccessResult(responseDto, HttpStatusCode.OK);
        }
    }
    public class ActivateDeactivateAmenityRequestDto
    {
        public int AmenityId { get; set; }
        public bool IsActive { get; set; }
    }
}

