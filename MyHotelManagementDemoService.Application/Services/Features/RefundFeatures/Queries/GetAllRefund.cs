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

namespace MyHotelManagementDemoService.Application.Services.Features.RefundFeatures.Queries
{
    public class GetRefunds : IRequest<Result<List<GetRefundsResponseDto>>>
    {
    }
    public class GetRefundsHandler : IRequestHandler<GetRefunds, Result<List<GetRefundsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRefundsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetRefundsResponseDto>>> Handle(GetRefunds request, CancellationToken cancellationToken)
        {
            var refunds = await _unitOfWork.refundRepository.GetAllAsync();

            var refundDtos = refunds.Select(refund => new GetRefundsResponseDto
            {
                Id = refund.Id,
                ReferenceDetails = refund.ReferenceDetails,
                Amount = refund.Amount,
                DateIssued = refund.DateIssued
            }).ToList();

            return Result<List<GetRefundsResponseDto>>.SuccessResult(refundDtos);
        }
    }


}
