using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.RefundFeatures.Command
{
    public class CreateRefund : IRequest<Result<CreateRefundResponseDto>>
    {
        public CreateRefundRequestDto RequestDto { get; }

        public CreateRefund(CreateRefundRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
    }
    public class CreateRefundHandler : IRequestHandler<CreateRefund, Result<CreateRefundResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRefundHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateRefundResponseDto>> Handle(CreateRefund request, CancellationToken cancellationToken)
        {
            var refundEntity = new Refund
            {
                ReferenceDetails = request.RequestDto.ReferenceDetails,
                Amount = request.RequestDto.Amount,
                DateIssued = DateTime.UtcNow // Set the current date and time
            };

            await _unitOfWork.refundRepository.AddAsync(refundEntity);
            await _unitOfWork.Save();

            var responseDto = new CreateRefundResponseDto
            {
                Id = refundEntity.Id,
                ReferenceDetails = refundEntity.ReferenceDetails,
                Amount = refundEntity.Amount,
                DateIssued = refundEntity.DateIssued
            };

            return Result<CreateRefundResponseDto>.SuccessResult(responseDto, HttpStatusCode.Created);
        }
    }
    public class CreateRefundRequestDto
    {
        public string ReferenceDetails { get; set; }
        public decimal Amount { get; set; }
    }
    public class CreateRefundResponseDto
    {
        public int Id { get; set; }
        public string ReferenceDetails { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateIssued { get; set; }
    }
}
