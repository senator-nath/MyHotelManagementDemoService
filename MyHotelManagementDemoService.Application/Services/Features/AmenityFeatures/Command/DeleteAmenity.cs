using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Command
{
    public class DeleteAmenity : IRequest<Result<Unit>>
    {
        public int Id { get; }

        public DeleteAmenity(int id)
        {
            Id = id;
        }
    }
    public class DeleteAmenityHandler : IRequestHandler<DeleteAmenity, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAmenityHandler> _logger;

        public DeleteAmenityHandler(IUnitOfWork unitOfWork, ILogger<DeleteAmenityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteAmenity request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting amenity");

                var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.Id);

                if (amenityEntity == null)
                {
                    _logger.LogError("Amenity not found");
                    return Result<Unit>.NotFound("Amenity not found");
                }

                _unitOfWork.amenityRepository.Delete(amenityEntity);
                await _unitOfWork.Save();

                _logger.LogInformation("Amenity deleted successfully");

                return Result<Unit>.SuccessResult("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting amenity");
                return Result<Unit>.InternalServerError();
            }
        }
    }
}
