using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHotelManagementDemoService.Domain.Entities;
using MyHotelManagementDemoService.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence
{
    public static class PersistenceRegistrationService
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration config)
        {


            services.AddDbContext<HotelManagementDbContext>(Options => Options.UseSqlServer(config.GetConnectionString("defaultConnection")));
            services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<HotelManagementDbContext>()
        .AddDefaultTokenProviders();
            return services;

        }
    }
}
