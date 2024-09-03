﻿using BlogApp.Application.Helpers;
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

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command
{
    public class CreateRoom : IRequest<Result<CreateRoomResponseDto>>
    {
        public CreateRoomRequestDto RequestDto { get; }

        public CreateRoom(CreateRoomRequestDto _requestDto)
        {
            RequestDto = _requestDto;
        }
    }
    public class CreateRoomHandler : IRequestHandler<CreateRoom, Result<CreateRoomResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateRoomHandler(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CreateRoomResponseDto>> Handle(CreateRoom request, CancellationToken cancellationToken)
        {
            try
            {

                var roomEntity = new Room
                {
                    RoomNumber = request.RequestDto.RoomNumber,
                    Price = request.RequestDto.Price,
                    Status = request.RequestDto.Status,
                    DateCreated = DateTime.UtcNow,
                    RoomTypeId = request.RequestDto.RoomTypeId,
                    RoomAmenitiesId = request.RequestDto.RoomAmenitiesId
                };

                await _unitOfWork.roomRepository.AddAsync(roomEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Room created: {RoomId}", roomEntity.Id);

                var responseDto = new CreateRoomResponseDto
                {
                    RoomId = roomEntity.Id,
                    RoomNumber = roomEntity.RoomNumber,
                    Price = roomEntity.Price,
                    Status = roomEntity.Status,
                    DateCreated = roomEntity.DateCreated,
                    RoomTypeId = roomEntity.RoomTypeId,
                    RoomAmenitiesId = roomEntity.RoomAmenitiesId
                };

                return Result<CreateRoomResponseDto>.SuccessResult(responseDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                return Result<CreateRoomResponseDto>.ErrorResult("Error creating room", HttpStatusCode.InternalServerError);
            }
        }

    }
    public class CreateRoomRequestDto
    {
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
    public class CreateRoomResponseDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }

}
