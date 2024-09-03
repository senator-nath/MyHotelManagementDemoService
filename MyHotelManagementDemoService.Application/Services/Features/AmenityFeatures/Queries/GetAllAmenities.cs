using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.AmenityFeatures.Queries
{
    public class GetAmenities : IRequest<Result<List<GetAmenitiesResponseDto>>>
    {
    }
    public class GetAmenitiesHandler : IRequestHandler<GetAmenities, Result<List<GetAmenitiesResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAmenitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAmenitiesResponseDto>>> Handle(GetAmenities request, CancellationToken cancellationToken)
        {
            var amenities = await _unitOfWork.amenityRepository.GetAllAsync();

            var amenityDtos = amenities.Select(amenity => new GetAmenitiesResponseDto
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsActive = amenity.IsActive,
                 RoomAmenitiesId = amenity.RoomAmenitiesId
            }).ToList();

            return Result<List<GetAmenitiesResponseDto>>.SuccessResult(amenityDtos, HttpStatusCode.OK);
        }
    }


}


