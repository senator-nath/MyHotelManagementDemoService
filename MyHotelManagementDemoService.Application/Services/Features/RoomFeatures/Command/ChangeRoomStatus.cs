using BlogApp.Application.Helpers;
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
        private readonly ILogger<ChangeRoomStatusHandler> _logger;

        public ChangeRoomStatusHandler(IUnitOfWork unitOfWork, ILogger<ChangeRoomStatusHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ChangeRoomStatusResponseDto>> Handle(ChangeRoomStatus request, CancellationToken cancellationToken)
        {
            try
            {

                var roomEntity = await _unitOfWork.roomRepository.GetByIdAsync(request.RequestDto.RoomId);

                if (roomEntity == null)
                {
                    _logger.LogWarning("Room not found: {RoomId}", request.RequestDto.RoomId);
                    return Result<ChangeRoomStatusResponseDto>.NotFound("Room not found");
                }

                var oldStatus = roomEntity.Status;

                roomEntity.Status = request.RequestDto.NewStatus;

                _unitOfWork.roomRepository.Update(roomEntity);
                await _unitOfWork.Save();

                var responseDto = new ChangeRoomStatusResponseDto
                {
                    RoomId = roomEntity.Id,
                    RoomNumber = roomEntity.RoomNumber,
                    OldStatus = oldStatus,
                    NewStatus = roomEntity.Status
                };

                _logger.LogInformation("Room status changed: {RoomId} - {OldStatus} -> {NewStatus}", roomEntity.Id, oldStatus, roomEntity.Status);

                return Result<ChangeRoomStatusResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing room status: {RoomId}", request.RequestDto.RoomId);
                return Result<ChangeRoomStatusResponseDto>.InternalServerError();
            }
        }

    }

    public class ChangeRoomStatusResponseDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
    }
    public class ChangeRoomStatusRequestDto
    {
        public int RoomId { get; set; }
        public string NewStatus { get; set; }
    }
}
