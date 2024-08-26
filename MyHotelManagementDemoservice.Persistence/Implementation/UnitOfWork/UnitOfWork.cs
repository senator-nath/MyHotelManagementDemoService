using HotelManagement.Application.Contracts.Repository;
using HotelManagement.Persistence.RepositoryImplementation.Repository;
using MyHotelManagementDemoService.Application.Contracts.RepositoryContracts;
using MyHotelManagementDemoService.Application.Contracts.UnitofWork;
using MyHotelManagementDemoService.Persistence.Data;
using MyHotelManagementDemoService.Persistence.Implementation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence.Implementation.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelManagementDbContext _dbContext;
        public IUserRepository userRepository { get; }

        public IAmenityRepository amenityRepository { get; }


        public IRoomRepository roomRepository { get; }

        public IRoomTypeRepository roomTypeRepository { get; }

        public IRefundRepository refundRepository { get; }



        public IBookingRepository bookingRepository { get; }

        public IFeedbackRepository feedbackRepository { get; }

        public IRoomAmenitiesRepository roomAmenitiesRepository { get; }
        public IStateRepository stateRepository { get; }

        public UnitOfWork(HotelManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            userRepository = new UserRepository(dbContext);
            amenityRepository = new AmenityRepository(dbContext);
            roomRepository = new RoomRepository(dbContext);
            roomTypeRepository = new RoomTypeRepository(dbContext);
            refundRepository = new RefundRepository(dbContext);
            bookingRepository = new BookingRepository(dbContext);
            feedbackRepository = new FeedbackRepository(dbContext);
            roomAmenitiesRepository = new RoomAmenitiesRepository(dbContext);
            stateRepository = new StateRepository(dbContext);

        }
        public void Dispose()
        {
            _dbContext.DisposeAsync();
        }

        public async Task<int> Save()
        {
            try
            {
                var save = await _dbContext.SaveChangesAsync();
                return save;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
