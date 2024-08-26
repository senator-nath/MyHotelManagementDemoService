using HotelManagement.Application.Contracts.Repository;
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
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        public StateRepository(HotelManagementDbContext _context) : base(_context)
        {

        }
    }
}
