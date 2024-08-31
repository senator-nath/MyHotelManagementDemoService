using BlogApp.Application.Helpers;
using MediatR;
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

        public UpdateAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateAmenityResponseDto>> Handle(UpdateAmenity request, CancellationToken cancellationToken)
        {
            var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.Id);

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
    }




}

