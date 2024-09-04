using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteRoomHandler> _logger;

        public DeleteRoomHandler(IUnitOfWork unitOfWork, ILogger<DeleteRoomHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteRoom request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.Id <= 0)
                {
                    _logger.LogWarning("Invalid room ID: {Id}", request.Id);
                    return Result<Unit>.ErrorResult("Invalid room ID", HttpStatusCode.BadRequest);
                }

                var roomEntity = await _unitOfWork.roomRepository.GetByColumnAsync(r => r.Id == request.Id);

                if (roomEntity == null)
                {
                    _logger.LogWarning("Room not found: {Id}", request.Id);
                    return Result<Unit>.ErrorResult("Room not found", HttpStatusCode.NotFound);
                }

                _unitOfWork.roomRepository.Delete(roomEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Room deleted: {Id}", request.Id);

                return Result<Unit>.SuccessResult(Unit.Value, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room: {Id}", request.Id);
                return Result<Unit>.ErrorResult("Error deleting room", HttpStatusCode.InternalServerError);
            }
        }
    }
}
