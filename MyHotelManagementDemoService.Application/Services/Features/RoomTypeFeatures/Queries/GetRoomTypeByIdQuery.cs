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
    public class GetRoomTypeByIdQuery : IRequest<Result<RoomTypeResponseDto>>
    {
        internal readonly int id;

        public GetRoomTypeByIdQuery(int id)
        {
            this.id = id;
        }
    }

    public class GetRoomTypeByIdQueryHandler : IRequestHandler<GetRoomTypeByIdQuery, Result<RoomTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetRoomTypeByIdQueryHandler> _logger;

        public GetRoomTypeByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetRoomTypeByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<RoomTypeResponseDto>> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting room type by id");

                var exist = await _unitOfWork.roomTypeRepository.GetByColumnAsync(x => x.Id == request.id);
                if (exist == null)
                {
                    _logger.LogWarning("Room type not found");
                    return Result<RoomTypeResponseDto>.NotFound("Room type not found");
                }

                var response = new RoomTypeResponseDto()
                {
                    Id = exist.Id,
                    TypeName = exist.TypeName,
                    Description = exist.Description,
                    AccessibilityFeatures = exist.AccessibilityFeatures,
                };

                _logger.LogInformation("Room type retrieved successfully");

                return Result<RoomTypeResponseDto>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room type by id");
                return Result<RoomTypeResponseDto>.InternalServerError();
            }
        }
    }
}