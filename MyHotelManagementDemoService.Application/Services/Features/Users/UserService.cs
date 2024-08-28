using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyHotelManagementDemoService.Application.Dtos.Request;
using MyHotelManagementDemoService.Application.Helper;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Services.Features.Users
{
    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> AddRole(string role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (roleExist)
            {
                return "Role already exist";
            }

            var addRole = await _roleManager.CreateAsync(new IdentityRole(role));
            if (addRole.Succeeded)
            {
                return "Role added successfully";
            }

            return "Something went wrong";
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }


        public async Task<string> AddUser(RegisterUserRequestDto userDto)
        {

            var newUser = new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                UserName = userDto.UserName,
            };
            var addUser = await _userManager.CreateAsync(newUser, userDto.Password);
            if (addUser.Succeeded)
            {

                return "User Successfully Created";
            }

            return "Bad Request";
        }

        public async Task<string> AddRoleToUser(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "Bad Request";
            }

            var addRole = await _userManager.AddToRoleAsync(user, role);
            if (addRole.Succeeded)
            {
                return "Role Added Successfully";
            }

            return "Something went wrong";
        }
        public async Task<IList<string>> GetUserRole(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new List<string>();
            }
            var userRole = await _userManager.GetRolesAsync(user);
            return userRole;
        }

        public async Task<string> RemoveUserRole(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "Bad Request";
            }
            var removeUserRole = await _userManager.RemoveFromRoleAsync(user, role);
            if (removeUserRole.Succeeded)
            {
                return "Success";
            }

            return "Something Went Wrong";
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return "Bad request";
            }

            var token = await JwtTokenGenerator.GetToken(_userManager, _configuration, user);
            var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            return finalToken;
        }
    }
}
