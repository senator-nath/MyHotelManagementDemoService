using BlogApp.Application.Helpers;
using MediatR;
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
        public CreateRoomHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateRoomResponseDto>> Handle(CreateRoom request, CancellationToken cancellationToken)
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
    }


}
