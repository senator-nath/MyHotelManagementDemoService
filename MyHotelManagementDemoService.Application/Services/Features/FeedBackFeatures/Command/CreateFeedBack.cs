using BlogApp.Application.Helpers;
using MediatR;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Dtos.Response;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.FeedBackFeatures.Command
{
    public class CreateFeedback : IRequest<Result<CreateFeedbackResponseDto>>
    {
        public CreateFeedbackRequestDto RequestDto { get; }

        public CreateFeedback(CreateFeedbackRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
    }
    public class CreateFeedbackHandler : IRequestHandler<CreateFeedback, Result<CreateFeedbackResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateFeedbackResponseDto>> Handle(CreateFeedback request, CancellationToken cancellationToken)
        {
            var feedbackEntity = new FeedBack
            {
                Comments = request.RequestDto.Comments,
                Rating = request.RequestDto.Rating,
                DateSubmitted = DateTime.UtcNow, // Set the current date and time
                UserId = request.RequestDto.UserId
            };

            await _unitOfWork.feedbackRepository.AddAsync(feedbackEntity);
            await _unitOfWork.Save();

            var responseDto = new CreateFeedbackResponseDto
            {
                Id = feedbackEntity.Id,
                Comments = feedbackEntity.Comments,
                Rating = feedbackEntity.Rating,
                DateSubmitted = feedbackEntity.DateSubmitted,
                UserId = feedbackEntity.UserId
            };

            return Result<CreateFeedbackResponseDto>.SuccessResult(responseDto, HttpStatusCode.Created);
        }
    }


}
