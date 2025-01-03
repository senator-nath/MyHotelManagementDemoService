﻿using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.FileStorage;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Dtos.Response;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ILogger<CreateRoomHandler> _logger;
        private readonly IFileStorageService _fileStorageService;

        public CreateRoomHandler(IUnitOfWork unitOfWork, ILogger<CreateRoomHandler> logger, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<CreateRoomResponseDto>> Handle(CreateRoom request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating room");

                var existingRoom = await _unitOfWork.roomRepository.GetByColumnAsync(r => r.RoomNumber == request.RequestDto.RoomNumber);

                if (existingRoom != null)
                {
                    _logger.LogError("Room already exists");
                    return Result<CreateRoomResponseDto>.Conflict("Room already exists");
                }
                var imageUrls = new List<string>();
                if (request.RequestDto.Images != null && request.RequestDto.Images.Count > 0)
                {
                    imageUrls = await _fileStorageService.UploadFilesAsync(request.RequestDto.Images, "images/rooms");
                }

                var roomEntity = new Room
                {
                    RoomNumber = request.RequestDto.RoomNumber,
                    Price = request.RequestDto.Price,
                    Status = request.RequestDto.Status,
                    DateCreated = DateTime.UtcNow,
                    RoomTypeId = request.RequestDto.RoomTypeId,
                    RoomAmenitiesId = request.RequestDto.RoomAmenitiesId,
                    Url = imageUrls
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
                    RoomAmenitiesId = roomEntity.RoomAmenitiesId,
                    Url = roomEntity.Url

                };

                return Result<CreateRoomResponseDto>.SuccessResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                return Result<CreateRoomResponseDto>.InternalServerError();
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
        public List<IFormFile> Images { get; set; }
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
        public List<string> Url { get; set; }
    }

}
