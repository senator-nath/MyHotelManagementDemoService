using System;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Command.RoomType
{
    public class CreateRoomTypeCommand : IRequest<Result<RoomTypeResponseDto>>
    {
        internal readonly RoomTypeRequestDto requestDto;

        public CreateRoomTypeCommand(RoomTypeRequestDto requestDto)
        {
            this.requestDto = requestDto;
        }
    }


    public class CreateRoomTypeHandler : IRequestHandler<CreateRoomTypeCommand, Result<RoomTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateRoomTypeHandler> _logger;

        public CreateRoomTypeHandler(IUnitOfWork unitOfWork, ILogger<CreateRoomTypeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<RoomTypeResponseDto>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating room type");

                var exist = await _unitOfWork.roomTypeRepository.GetByColumnAsync(x => x.TypeName == request.requestDto.TypeName);

                if (exist != null)
                {
                    _logger.LogError("Room type already exists");
                    return Result<RoomTypeResponseDto>.Conflict("Room type already exists");
                }

                var newAmenity = new MyHotelManagementDemoService.Domain.Entities.RoomType()
                {
                    TypeName = request.requestDto.TypeName,
                    Description = request.requestDto.Description,
                    AccessibilityFeatures = request.requestDto.AccessibilityFeatures,
                };

                await _unitOfWork.roomTypeRepository.AddAsync(newAmenity);
                var save = await _unitOfWork.Save();

                if (save < 1)
                {
                    _logger.LogError("Error creating room type");
                    return Result<RoomTypeResponseDto>.InternalServerError();
                }

                _logger.LogInformation("Room type created successfully");

                var response = new RoomTypeResponseDto()
                {
                    Id = newAmenity.Id,
                    AccessibilityFeatures = newAmenity.AccessibilityFeatures,
                    TypeName = newAmenity.TypeName,
                    Description = newAmenity.Description
                };

                return Result<RoomTypeResponseDto>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room type");
                return Result<RoomTypeResponseDto>.InternalServerError();
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
    public class RoomTypeRequestDto
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string AccessibilityFeatures { get; set; }
    }
}