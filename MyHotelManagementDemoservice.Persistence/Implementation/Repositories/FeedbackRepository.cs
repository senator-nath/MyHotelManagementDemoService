using HotelManagement.Application.Contracts.Repository;
using MyHotelManagementDemoService.Domain.Entities;
using MyHotelManagementDemoService.Persistence.Data;
using MyHotelManagementDemoService.Persistence.Implementation.GenericRepositoryImplementation;


namespace HotelManagement.Persistence.RepositoryImplementation.Repository
{
    public class FeedbackRepository : GenericRepository<FeedBack>, IFeedbackRepository
    {
        public FeedbackRepository(HotelManagementDbContext _context) : base(_context)
        {

        }
    }
}