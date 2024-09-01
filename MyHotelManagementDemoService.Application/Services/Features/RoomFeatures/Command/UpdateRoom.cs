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

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command
{
    public class UpdateRoom : IRequest<Result<UpdateRoomResponseDto>>
    {
        public int Id { get; }
        public UpdateRoomRequestDto RequestDto { get; }

        public UpdateRoom(int id, UpdateRoomRequestDto requestDto)
        {
            Id = id;
            RequestDto = requestDto;
        }
    }
    public class UpdateRoomHandler : IRequestHandler<UpdateRoom, Result<UpdateRoomResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateRoomResponseDto>> Handle(UpdateRoom request, CancellationToken cancellationToken)
        {
            var roomEntity = await _unitOfWork.roomRepository.GetByColumnAsync(a => a.Id == request.Id);

            if (roomEntity == null)
            {
                return Result<UpdateRoomResponseDto>.ErrorResult("Room not found", HttpStatusCode.NotFound);
            }

            // Update room entity with the new details from RequestDto
            roomEntity.RoomNumber = request.RequestDto.RoomNumber;
            roomEntity.Price = request.RequestDto.Price;
            roomEntity.Status = request.RequestDto.Status;
            roomEntity.RoomTypeId = request.RequestDto.RoomTypeId;
            roomEntity.RoomAmenitiesId = request.RequestDto.RoomAmenitiesId;

            _unitOfWork.roomRepository.Update(roomEntity);
            await _unitOfWork.Save();

            var responseDto = new UpdateRoomResponseDto
            {
                Id = roomEntity.Id,
                RoomNumber = roomEntity.RoomNumber,
                Price = roomEntity.Price,
                Status = roomEntity.Status,
                DateUpdated = roomEntity.DateCreated, // Assuming you want the DateCreated as DateUpdated
                RoomTypeId = roomEntity.RoomTypeId,
                RoomAmenitiesId = roomEntity.RoomAmenitiesId
            };

            return Result<UpdateRoomResponseDto>.SuccessResult(responseDto, HttpStatusCode.OK);
        }
    }


}
