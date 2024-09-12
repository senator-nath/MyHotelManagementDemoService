using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{
    //public class AssignAmenityToRoomCommand : IRequest<Result<Unit>>
    //{
    //    internal readonly int RoomAmenitiesId;
    //    internal readonly int AmenityId;

    //    public AssignAmenityToRoomCommand(int roomAmenitiesId, int amenityId)
    //    {
    //        RoomAmenitiesId = roomAmenitiesId;
    //        AmenityId = amenityId;
    //    }
    //}

    //public class AssignAmenityToRoomHandler : IRequestHandler<AssignAmenityToRoomCommand, Result<Unit>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public AssignAmenityToRoomHandler(IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<Result<Unit>> Handle(AssignAmenityToRoomCommand request, CancellationToken cancellationToken)
    //    {
    //        var roomAmenities = await _unitOfWork.roomAmenitiesRepository.GetByColumnAsync(x => x.Id == request.RoomAmenitiesId);

    //        if (roomAmenities == null)
    //        {
    //            return Result<Unit>.NotFound("Room Amenities not found");
    //        }

    //        var amenity = await _unitOfWork.amenityRepository.GetByColumnAsync(x => x.Id == request.AmenityId);

    //        if (amenity == null)
    //        {
    //            return Result<Unit>.NotFound("Amenity not found");
    //        }

    //        roomAmenities.Amenities.Add(amenity);
    //        _unitOfWork.roomAmenitiesRepository.Update(roomAmenities);
    //        await _unitOfWork.Save();

    //        return Result<Unit>.SuccessResult("Amenity assigned to Room Amenities successfully");
    //    }
    //}
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

        public AssignAmenityToRoomAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AssignAmenityToRoomAmenityResponseDto>> Handle(AssignAmenityToRoomAmenity request, CancellationToken cancellationToken)
        {
            var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.AmenityId);

            if (amenityEntity == null)
            {
                return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Amenity not found");
            }

            if (!amenityEntity.IsActive)
            {
                return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Amenity is not active");
            }

            var roomAmenityEntity = await _unitOfWork.roomAmenitiesRepository.GetByColumnAsync(a => a.Id == request.RoomAmenitiesId);

            if (roomAmenityEntity == null)
            {
                return Result<AssignAmenityToRoomAmenityResponseDto>.NotFound("Room amenity not found");
            }

            roomAmenityEntity.Amenities.Add(amenityEntity);
            _unitOfWork.roomAmenitiesRepository.Update(roomAmenityEntity);
            await _unitOfWork.Save();

            var responseDto = new AssignAmenityToRoomAmenityResponseDto
            {
                AmenityId = amenityEntity.Id,
                RoomAmenitiesId = roomAmenityEntity.Id
            };

            return Result<AssignAmenityToRoomAmenityResponseDto>.SuccessResult(responseDto);
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
