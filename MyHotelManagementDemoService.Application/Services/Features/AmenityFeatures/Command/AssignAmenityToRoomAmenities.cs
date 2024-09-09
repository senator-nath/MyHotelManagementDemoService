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
    public class AssignAmenityToRoomCommand : IRequest<Result<Unit>>
    {
        internal readonly int RoomAmenitiesId;
        internal readonly int AmenityId;

        public AssignAmenityToRoomCommand(int roomAmenitiesId, int amenityId)
        {
            RoomAmenitiesId = roomAmenitiesId;
            AmenityId = amenityId;
        }
    }

    public class AssignAmenityToRoomHandler : IRequestHandler<AssignAmenityToRoomCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignAmenityToRoomHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(AssignAmenityToRoomCommand request, CancellationToken cancellationToken)
        {
            var roomAmenities = await _unitOfWork.roomAmenitiesRepository.GetByColumnAsync(x => x.Id == request.RoomAmenitiesId);

            if (roomAmenities == null)
            {
                return Result<Unit>.NotFound("Room Amenities not found");
            }

            var amenity = await _unitOfWork.amenityRepository.GetByColumnAsync(x => x.Id == request.AmenityId);

            if (amenity == null)
            {
                return Result<Unit>.NotFound("Amenity not found");
            }

            roomAmenities.Amenities.Add(amenity);
            _unitOfWork.roomAmenitiesRepository.Update(roomAmenities);
            await _unitOfWork.Save();

            return Result<Unit>.SuccessResult("Amenity assigned to Room Amenities successfully");
        }
    }
}
