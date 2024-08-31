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
    public class ChangeRoomStatus : IRequest<Result<ChangeRoomStatusResponseDto>>
    {
        public ChangeRoomStatusRequestDto RequestDto { get; }

        public ChangeRoomStatus(ChangeRoomStatusRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
    }
    public class ChangeRoomStatusHandler : IRequestHandler<ChangeRoomStatus, Result<ChangeRoomStatusResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeRoomStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ChangeRoomStatusResponseDto>> Handle(ChangeRoomStatus request, CancellationToken cancellationToken)
        {
            // Fetch the room entity by its ID
            var roomEntity = await _unitOfWork.roomRepository.GetByIdAsync(request.RequestDto.RoomId);

            if (roomEntity == null)
            {
                return Result<ChangeRoomStatusResponseDto>.ErrorResult("Room not found.", HttpStatusCode.NotFound);
            }

            // Store the old status for the response
            var oldStatus = roomEntity.Status;

            // Update the room's status
            roomEntity.Status = request.RequestDto.NewStatus;

            // Update the room entity in the repository
            _unitOfWork.roomRepository.Update(roomEntity);
            await _unitOfWork.Save();

            // Prepare the response DTO
            var responseDto = new ChangeRoomStatusResponseDto
            {
                RoomId = roomEntity.Id,
                RoomNumber = roomEntity.RoomNumber,
                OldStatus = oldStatus,
                NewStatus = roomEntity.Status
            };

            return Result<ChangeRoomStatusResponseDto>.SuccessResult(responseDto, HttpStatusCode.OK);
        }
    }



}
