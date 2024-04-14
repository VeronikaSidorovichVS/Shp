using Microsoft.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Fluent.Infrastructure.FluentModel;


namespace Shops.Login
{
    namespace RolesIdentityApp.Models
    {
        public class UserRoleService
        {
            private UserManager<ApplicationUser> _userManager;
            private RoleManager<IdentityRole> _roleManager;

            public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<bool> AssignRoleToUser(string userId, string roleName)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return false;
                }


                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {

                    var newRole = new IdentityRole(roleName);
                    var result = await _roleManager.CreateAsync(newRole);
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }


                var assignResult = await _userManager.AddToRoleAsync(user, roleName);
                if (assignResult.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //public class ApplicationUser
        //{
        //}

    }
}