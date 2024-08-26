using Microsoft.AspNetCore.Identity;
using MyHotelManagementDemoService.Application.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.Users
{
    public interface IUserService
    {
        Task<string> AddRole(string role);
        Task<List<IdentityRole>> GetRoles();

        Task<string> AddUser(RegisterUserRequestDto userDto);
        Task<string> AddRoleToUser(string useName, string role);
        Task<IList<string>> GetUserRole(string userName);
        Task<string> RemoveUserRole(string userName, string role);
        Task<string> Login(string userName, string password);
    }
}
