using BlogApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public DeleteAmenityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteAmenity request, CancellationToken cancellationToken)
        {
            var amenityEntity = await _unitOfWork.amenityRepository.GetByColumnAsync(a => a.Id == request.Id);

            if (amenityEntity == null)
            {
                return Result<Unit>.ErrorResult("Amenity not found", HttpStatusCode.NotFound);
            }

            _unitOfWork.amenityRepository.Delete(amenityEntity);
            await _unitOfWork.Save();

            return Result<Unit>.SuccessResult(Unit.Value, HttpStatusCode.OK);
        }
    }
}
