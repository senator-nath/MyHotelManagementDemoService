using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;

namespace HotelManagement.Application.Command.RoomType
{
    public class DeleteRoomTypeCommand : IRequest<Result<Unit>>
    {
        public int Id { get; }

        public DeleteRoomTypeCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Unit>> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var exist = await _unitOfWork.roomTypeRepository.GetByColumnAsync(x => x.Id == request.Id);
            if (exist == null)
            {
                return Result<Unit>.NotFound("RoomType not found");
            }

            _unitOfWork.roomTypeRepository.Delete(exist);
            var save = await _unitOfWork.Save();

            if (save < 1)
            {
                return Result<Unit>.InternalServerError();
            }

            return Result<Unit>.SuccessResult("Success");
        }

    }
}