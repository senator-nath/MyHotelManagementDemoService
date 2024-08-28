using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
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

        public CreateAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateAmenityResponseDto>> Handle(CreateAmenity request, CancellationToken cancellationToken)
        {
            var amenityEntity = new Amenity
            {
                Name = request.RequestDto.Name,
                Description = request.RequestDto.Description,
                IsActive = request.RequestDto.IsActive,
                RoomAmenitiesId = request.RequestDto.RoomAmenitiesId
            };

            await _unitOfWork.amenityRepository.AddAsync(amenityEntity);
            await _unitOfWork.Save();

            var responseDto = new CreateAmenityResponseDto
            {
                AmenityId = amenityEntity.Id,
                Name = amenityEntity.Name,
                Description = amenityEntity.Description,
                IsActive = amenityEntity.IsActive,
                RoomAmenitiesId = amenityEntity.RoomAmenitiesId
            };

            return Result<CreateAmenityResponseDto>.SuccessResult(responseDto, HttpStatusCode.OK);
        }
    }
    public class CreateAmenityRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
    public class CreateAmenityResponseDto
    {
        public int AmenityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}
