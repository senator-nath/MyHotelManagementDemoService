using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
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
        public UpdateAmenityRequestDto RequestDto { get; }

        public UpdateAmenity(UpdateAmenityRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
    }
    public class UpdateAmenityHandler : IRequestHandler<UpdateAmenity, Result<UpdateAmenityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateAmenityResponseDto>> Handle(UpdateAmenity request, CancellationToken cancellationToken)
        {
            var amenityEntity = await _unitOfWork.amenityRepository.GetByIdAsync(request.RequestDto.Id);

            if (amenityEntity == null)
            {
                return Result<UpdateAmenityResponseDto>.ErrorResult("Amenity not found", HttpStatusCode.NotFound);
            }

            // Update amenity entity with the new details from RequestDto
            amenityEntity.Name = request.RequestDto.Name;
            amenityEntity.Description = request.RequestDto.Description;
            amenityEntity.IsActive = request.RequestDto.IsActive;
            amenityEntity.RoomAmenitiesId = request.RequestDto.RoomAmenitiesId;

            _unitOfWork.amenityRepository.Update(amenityEntity);
            await _unitOfWork.Save();

            var responseDto = new UpdateAmenityResponseDto
            {
                Id = amenityEntity.Id,
                Name = amenityEntity.Name,
                Description = amenityEntity.Description,
                IsActive = amenityEntity.IsActive,
                RoomAmenitiesId = amenityEntity.RoomAmenitiesId
            };

            return Result<UpdateAmenityResponseDto>.SuccessResult(responseDto, HttpStatusCode.OK);
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
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public int RoomAmenitiesId { get; set; }
        }
    }
}
