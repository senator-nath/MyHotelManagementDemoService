using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Query.RoomType
{
    public class GetAllRoomTypeQuery : IRequest<Result<IEnumerable<RoomTypeResponseDto>>>
    {

    }

    public class GetAllRoomTypeQueryHandler : IRequestHandler<GetAllRoomTypeQuery, Result<IEnumerable<RoomTypeResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllRoomTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<RoomTypeResponseDto>>> Handle(GetAllRoomTypeQuery request, CancellationToken cancellationToken)
        {
            var roomType = await _unitOfWork.roomTypeRepository.GetAllAsync();

            var roomTypeDto = roomType.Select
            (p => new RoomTypeResponseDto
            {
                TypeName = p.TypeName,
                Description = p.Description,
                AccessibilityFeatures = p.AccessibilityFeatures,
            });

            return Result<IEnumerable<RoomTypeResponseDto>>.SuccessResult(roomTypeDto);
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