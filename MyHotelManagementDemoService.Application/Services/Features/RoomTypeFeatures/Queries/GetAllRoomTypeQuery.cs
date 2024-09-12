using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Query.RoomType
{
    public class GetAllRoomTypeQuery : IRequest<Result<IEnumerable<RoomTypeResponseDto>>>
    {

    }

    public class GetAllRoomTypeQueryHandler : IRequestHandler<GetAllRoomTypeQuery, Result<IEnumerable<RoomTypeResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllRoomTypeQueryHandler> _logger;

        public GetAllRoomTypeQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllRoomTypeQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<RoomTypeResponseDto>>> Handle(GetAllRoomTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting all room types");

                var roomType = await _unitOfWork.roomTypeRepository.GetAllAsync();

                var roomTypeDto = roomType.Select
                (p => new RoomTypeResponseDto
                {
                    TypeName = p.TypeName,
                    Description = p.Description,
                    AccessibilityFeatures = p.AccessibilityFeatures,
                });

                _logger.LogInformation("Room types retrieved successfully");

                return Result<IEnumerable<RoomTypeResponseDto>>.SuccessResult(roomTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room types");
                return Result<IEnumerable<RoomTypeResponseDto>>.InternalServerError();
            }
        }
    }
    public class RoomTypeResponseDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string AccessibilityFeatures { get; set; }
    }
}