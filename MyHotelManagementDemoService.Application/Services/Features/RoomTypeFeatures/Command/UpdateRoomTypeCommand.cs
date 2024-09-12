using System;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Command.RoomType
{
    public class UpdateRoomTypeCommand : IRequest<Result<RoomTypeResponseDto>>
    {
        internal readonly RoomTypeRequestDto requestDto;

        public UpdateRoomTypeCommand(RoomTypeRequestDto requestDto)
        {
            this.requestDto = requestDto;
        }
    }

    public class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeCommand, Result<RoomTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRoomTypeHandler> _logger;

        public UpdateRoomTypeHandler(IUnitOfWork unitOfWork, ILogger<UpdateRoomTypeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<RoomTypeResponseDto>> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating room type");

                var exist = await _unitOfWork.roomTypeRepository.GetByColumnAsync(x => x.TypeName == request.requestDto.TypeName);
                if (exist == null)
                {
                    _logger.LogWarning("RoomType not found");
                    return Result<RoomTypeResponseDto>.NotFound("RoomType not found");
                }

                exist.TypeName = request.requestDto.TypeName;
                exist.Description = request.requestDto.Description;
                exist.AccessibilityFeatures = request.requestDto.AccessibilityFeatures;
                _unitOfWork.roomTypeRepository.Update(exist);
                var save = await _unitOfWork.Save();
                if (save < 1)
                {
                    _logger.LogError("Error updating room type");
                    return Result<RoomTypeResponseDto>.InternalServerError();
                }

                _logger.LogInformation("Room type updated successfully");

                var response = new RoomTypeResponseDto()
                {
                    TypeName = request.requestDto.TypeName,
                    Description = request.requestDto.Description,
                    AccessibilityFeatures = request.requestDto.AccessibilityFeatures
                };

                return Result<RoomTypeResponseDto>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room type");
                return Result<RoomTypeResponseDto>.InternalServerError();
            }
        }
    }
}