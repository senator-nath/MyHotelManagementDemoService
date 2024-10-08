﻿using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command.UpdateAmenityHandler;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{
    public class UpdateAmenity : IRequest<Result<UpdateAmenityResponseDto>>
    {
        public int Id { get; }
        public UpdateAmenityRequestDto RequestDto { get; }

        public UpdateAmenity(int id, UpdateAmenityRequestDto requestDto)
        {
            Id = id;
            RequestDto = requestDto;
        }
    }




    public class UpdateAmenityHandler : IRequestHandler<UpdateAmenity, Result<UpdateAmenityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAmenityHandler> _logger;

        public UpdateAmenityHandler(IUnitOfWork unitOfWork, ILogger<UpdateAmenityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<UpdateAmenityResponseDto>> Handle(UpdateAmenity request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating amenity");

                var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.Id);

                if (amenityEntity == null)
                {
                    _logger.LogError("Amenity not found");
                    return Result<UpdateAmenityResponseDto>.NotFound("Amenity not found");
                }

                amenityEntity.Name = request.RequestDto.Name;
                amenityEntity.Description = request.RequestDto.Description;
                amenityEntity.IsActive = request.RequestDto.IsActive;
                amenityEntity.RoomAmenitiesId = request.RequestDto.RoomAmenitiesId;

                _unitOfWork.amenityRepository.Update(amenityEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Amenity updated successfully");

                var responseDto = new UpdateAmenityResponseDto
                {
                    Id = amenityEntity.Id,
                    Name = amenityEntity.Name,
                    Description = amenityEntity.Description,
                    IsActive = amenityEntity.IsActive,
                    RoomAmenitiesId = amenityEntity.RoomAmenitiesId
                };

                return Result<UpdateAmenityResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating amenity");
                return Result<UpdateAmenityResponseDto>.InternalServerError();
            }
        }
    }
    public class UpdateAmenityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
    public class UpdateAmenityRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }


}


