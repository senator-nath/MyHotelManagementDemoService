using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Response;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Queries
{
    public class GetAvailableRooms : IRequest<Result<List<GetAvailableRoomsResponseDto>>>
    {
    }

    public class GetAvailableRoomsHandler : IRequestHandler<GetAvailableRooms, Result<List<GetAvailableRoomsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableRoomsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAvailableRoomsResponseDto>>> Handle(GetAvailableRooms request, CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.roomRepository.GetWhereAndIncludeAsync(
                r => r.Status == "Available",
                include: r => r.Include(rt => rt.RoomType)
            );

            var roomDtos = rooms.Select(r => new GetAvailableRoomsResponseDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Price = r.Price,
                RoomTypeName = r.RoomType.TypeName
            }).ToList();

            return Result<List<GetAvailableRoomsResponseDto>>.SuccessResult(roomDtos, HttpStatusCode.OK);
        }
    }

    public class GetAvailableRoomsResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string RoomTypeName { get; set; }
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string RoomTypeName { get; set; }
    }

}
