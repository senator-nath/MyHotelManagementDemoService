 
using MyHotelManagementDemoService.Application.Contracts.GenericRepository;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Application.Contracts.Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        
    }
}