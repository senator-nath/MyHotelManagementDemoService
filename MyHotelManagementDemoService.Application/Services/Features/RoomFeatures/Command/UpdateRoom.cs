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
        private readonly ILogger<UpdateRoomHandler> _logger;

        public UpdateRoomHandler(IUnitOfWork unitOfWork, ILogger<UpdateRoomHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<UpdateRoomResponseDto>> Handle(UpdateRoom request, CancellationToken cancellationToken)
        {
            try
            {
                var roomEntity = await _unitOfWork.roomRepository.GetByColumnAsync(a => a.Id == request.Id);

                if (roomEntity == null)
                {
                    _logger.LogWarning("Room not found: {Id}", request.Id);
                    return Result<UpdateRoomResponseDto>.NotFound("Room not found");
                }

                roomEntity.RoomNumber = request.RequestDto.RoomNumber;
                roomEntity.Price = request.RequestDto.Price;
                roomEntity.Status = request.RequestDto.Status;
                roomEntity.RoomTypeId = request.RequestDto.RoomTypeId;
                roomEntity.RoomAmenitiesId = request.RequestDto.RoomAmenitiesId;

                _unitOfWork.roomRepository.Update(roomEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Room updated: {Id}", request.Id);

                var responseDto = new UpdateRoomResponseDto
                {
                    Id = roomEntity.Id,
                    RoomNumber = roomEntity.RoomNumber,
                    Price = roomEntity.Price,
                    Status = roomEntity.Status,
                    DateUpdated = DateTime.UtcNow,
                    RoomTypeId = roomEntity.RoomTypeId,
                    RoomAmenitiesId = roomEntity.RoomAmenitiesId
                };

                return Result<UpdateRoomResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room: {Id}", request.Id);
                return Result<UpdateRoomResponseDto>.InternalServerError();
            }
        }

    }
    public class UpdateRoomResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateUpdated { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
    public class UpdateRoomRequestDto
    {
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}