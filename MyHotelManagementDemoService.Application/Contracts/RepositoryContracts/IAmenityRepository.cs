using MyHotelManagementDemoService.Application.Contracts.GenericRepository;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HotelManagement.Application.Contracts.Repository
{
    public interface IAmenityRepository : IGenericRepository<Amenity>
    {

    }
}