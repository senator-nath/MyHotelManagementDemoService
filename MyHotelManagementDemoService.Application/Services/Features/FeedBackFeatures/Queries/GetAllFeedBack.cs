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

namespace MyHotelManagementDemoService.Application.Services.Features.FeedBackFeatures.Queries
{
    public class GetFeedbacks : IRequest<Result<List<GetFeedbacksResponseDto>>>
    {
    }
    public class GetFeedbacksHandler : IRequestHandler<GetFeedbacks, Result<List<GetFeedbacksResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFeedbacksHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetFeedbacksResponseDto>>> Handle(GetFeedbacks request, CancellationToken cancellationToken)
        {
            var feedbacks = await _unitOfWork.feedbackRepository.GetAllAsync();

            var feedbackDtos = feedbacks.Select(feedback => new GetFeedbacksResponseDto
            {
                Id = feedback.Id,
                Comments = feedback.Comments,
                Rating = feedback.Rating,
                DateSubmitted = feedback.DateSubmitted,
                UserId = feedback.UserId,
                UserName = feedback.User?.UserName
            }).ToList();

            return Result<List<GetFeedbacksResponseDto>>.SuccessResult(feedbackDtos, HttpStatusCode.OK);
        }
    }
    public class GetFeedbacksResponseDto
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
