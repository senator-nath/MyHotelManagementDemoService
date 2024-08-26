using MyHotelManagementDemoService.Application.Contracts.RepositoryContracts;
using MyHotelManagementDemoService.Domain.Entities;
using MyHotelManagementDemoService.Persistence.Data;
using MyHotelManagementDemoService.Persistence.Implementation.GenericRepositoryImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence.Implementation.Repositories
{
    public class RoomAmenitiesRepository : GenericRepository<RoomAmenities>, IRoomAmenitiesRepository
    {
        public RoomAmenitiesRepository(HotelManagementDbContext _context) : base(_context)
        {

        }
    }
}
