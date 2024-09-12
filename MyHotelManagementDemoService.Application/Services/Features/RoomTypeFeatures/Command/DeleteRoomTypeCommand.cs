using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteRoomTypeCommandHandler> _logger;

        public DeleteRoomTypeCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRoomTypeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting room type");

                var exist = await _unitOfWork.roomTypeRepository.GetByColumnAsync(x => x.Id == request.Id);

                if (exist == null)
                {
                    _logger.LogError("Room type not found");
                    return Result<Unit>.NotFound("Room type not found");
                }

                _unitOfWork.roomTypeRepository.Delete(exist);
                var save = await _unitOfWork.Save();

                if (save < 1)
                {
                    _logger.LogError("Error deleting room type");
                    return Result<Unit>.InternalServerError();
                }

                _logger.LogInformation("Room type deleted successfully");

                return Result<Unit>.SuccessResult("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room type");
                return Result<Unit>.InternalServerError();
            }
        }
    }
}