using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RoomFeatures.Command
{
    public class DeleteRoom : IRequest<Result<Unit>>
    {
        public int Id { get; }

        public DeleteRoom(int id)
        {
            Id = id;
        }
    }
    public class DeleteRoomHandler : IRequestHandler<DeleteRoom, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteRoom request, CancellationToken cancellationToken)
        {
            var roomEntity = await _unitOfWork.roomRepository.GetByColumnAsync(r => r.Id == request.Id);

            if (roomEntity == null)
            {
                return Result<Unit>.ErrorResult("Room not found", HttpStatusCode.NotFound);
            }

            _unitOfWork.roomRepository.Delete(roomEntity);
            await _unitOfWork.Save();

            return Result<Unit>.SuccessResult(Unit.Value, HttpStatusCode.OK);
        }
    }
}
