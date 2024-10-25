using Aeternum.Entities.User;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var role = new ApplicationRole { Name = roleName, Description = description };
                return await _roleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                // Logování chyby
                Console.WriteLine($"Chyba při vytváření role: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při vytváření role." });
            }
        }

        // Získání všech rolí
        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            try
            {
                return await Task.FromResult(_roleManager.Roles.ToList());
            }
            catch (Exception ex)
            {
                // Logování chyby
                Console.WriteLine($"Chyba při získávání rolí: {ex.Message}");
                return new List<ApplicationRole>(); // Vrátíme prázdný seznam, pokud dojde k chybě
            }
        }

        // Přiřazení role uživateli
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Uživatel nebyl nalezen." });

                if (roleName == "Admin")
                {
                    // Admin bude mít přístup i jako "User"
                    await _userManager.AddToRoleAsync(user, "User");
                }

                return await _userManager.AddToRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                // Logování chyby
                Console.WriteLine($"Chyba při přiřazování role uživateli: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při přiřazování role uživateli." });
            }
        }

        // Odebrání role od uživatele
        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Uživatel nebyl nalezen." });

                return await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                // Logování chyby
                Console.WriteLine($"Chyba při odebrání role od uživatele: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při odebrání role od uživatele." });
            }
        }
    }
}
