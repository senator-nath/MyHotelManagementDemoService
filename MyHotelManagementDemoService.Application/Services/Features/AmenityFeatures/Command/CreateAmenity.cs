using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Dtos.Response;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{
    public class CreateAmenity : IRequest<Result<CreateAmenityResponseDto>>
    {
        public CreateAmenityRequestDto RequestDto { get; }

        public CreateAmenity(CreateAmenityRequestDto requestDto)
        {
            RequestDto = requestDto;
        }

    }
    public class CreateAmenityHandler : IRequestHandler<CreateAmenity, Result<CreateAmenityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAmenityHandler> _logger;

        public CreateAmenityHandler(IUnitOfWork unitOfWork, ILogger<CreateAmenityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CreateAmenityResponseDto>> Handle(CreateAmenity request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating amenity");

                var existingAmenity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Name == request.RequestDto.Name);

                if (existingAmenity != null)
                {
                    _logger.LogError("Amenity already exists");
                    return Result<CreateAmenityResponseDto>.Conflict("Amenity already exists");
                }

                var amenityEntity = new Amenity
                {
                    Name = request.RequestDto.Name,
                    Description = request.RequestDto.Description,
                    IsActive = request.RequestDto.IsActive,
                    RoomAmenitiesId = request.RequestDto.RoomAmenitiesId
                };

                await _unitOfWork.amenityRepository.AddAsync(amenityEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Amenity created successfully");

                var responseDto = new CreateAmenityResponseDto
                {
                    AmenityId = amenityEntity.Id,
                    Name = amenityEntity.Name,
                    Description = amenityEntity.Description,
                    IsActive = amenityEntity.IsActive,
                    RoomAmenitiesId = amenityEntity.RoomAmenitiesId
                };

                return Result<CreateAmenityResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating amenity");
                return Result<CreateAmenityResponseDto>.InternalServerError();
            }
        }
    }
    public class CreateAmenityResponseDto
    {
        public int AmenityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
    public class CreateAmenityRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}
