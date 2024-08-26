using MyHotelManagementDemoService.Application.Contracts.GenericRepository;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Contracts.RepositoryContracts
{
    public interface IStateRepository : IGenericRepository<State>
    {
    }
}
