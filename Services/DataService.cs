using OttBlog.Data;
using OttBlog.Models;
using OttBlog.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using OttBlog.ViewModels;
using OttBlog.Services;
using OttBlog.Services.Interfaces;

namespace OttBlog.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            await _dbContext.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        // step 1 seed roles
        private async Task SeedRolesAsync()
        {
            //are there any roles in the system?
            if (_dbContext.Roles.Any())
            {
                //if there is, break out
                return;
            }
            //if not, continue in this code block
            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        // step 2 seed users
        private async Task SeedUsersAsync()
        {
            //are there any users in the system?
            if (_dbContext.Users.Any())
            {
                //if there are already users, GTFO of this Task!
                return;
            }

            //otherwise, continue creating and instance of a user
            var adminUser = new BlogUser()
            {
                Email = "juggernott81@gmail.com",
                UserName = "juggernott81@gmail.com",
                FirstName = "Lawson",
                LastName = "Ott",
                DisplayName = "JuggernOtt81",
                PhoneNumber = "(555) 867-5309",
                EmailConfirmed = true,
            };

            //then define that user by the adminUser designation
            await _userManager.CreateAsync(adminUser, "Abc&123!");

            //then add this user to the role of admin
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());

            //repeat for moderator
            var moderatorUser = new BlogUser()
            {
                Email = "lawsonott3@gmail.com",
                UserName = "lawsonott3@gmail.com",
                FirstName = "Lawson",
                LastName = "Ott",
                DisplayName = "lawsonott3",
                PhoneNumber = "(800) 867-5309",
                EmailConfirmed = true,
            };
            await _userManager.CreateAsync(moderatorUser, "Abc&123!");
            await _userManager.AddToRoleAsync(moderatorUser, BlogRole.Moderator.ToString());
        }
    }
}
