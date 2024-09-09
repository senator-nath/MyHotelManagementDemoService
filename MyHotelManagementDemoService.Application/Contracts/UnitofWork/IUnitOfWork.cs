using HotelManagement.Application.Contracts.Repository;
using MyHotelManagementDemoService.Application.Contracts.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Contracts.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository userRepository { get; }
        IAmenityRepository amenityRepository { get; }
        IRoomAmenitiesRepository roomAmenitiesRepository { get; }
        IRoomRepository roomRepository { get; }
        IRoomTypeRepository roomTypeRepository { get; }
        IRefundRepository refundRepository { get; }

        IBookingRepository bookingRepository { get; }
        IFeedbackRepository feedbackRepository { get; }
        IStateRepository stateRepository { get; }
        IPaymentRepository paymentRepository { get; }

        Task<int> Save();
    }
}
