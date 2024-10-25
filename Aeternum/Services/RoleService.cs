using Aeternum.Entities.User;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aeternum.Services
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Vytvoření nové role
        public async Task<IdentityResult> CreateRoleAsync(string roleName, string description)
        {
            var role = new ApplicationRole { Name = roleName, Description = description };
            return await _roleManager.CreateAsync(role);
        }

        // Získání všech rolí
        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.ToList());
        }

        // Přiřazení role uživateli
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        // Odebrání role od uživatele
        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
