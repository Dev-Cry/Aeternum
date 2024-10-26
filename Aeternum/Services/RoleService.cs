using Aeternum.Entities.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                Console.WriteLine($"Chyba při vytváření role: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při vytváření role." });
            }
        }

        // Inicializace výchozích rolí s claimy
        public async Task InitializeRolesAsync()
        {
            var rolesWithClaims = new List<(string RoleName, string Description, List<Claim> Claims)>
            {
                ("Admin", "Role s plným přístupem k administraci systému", new List<Claim> { new Claim("Permission", "FullAccess") }),
                ("User", "Role pro běžné uživatele", new List<Claim> { new Claim("Permission", "ViewOnly") }),
                ("Manager", "Role s právy pro správu projektů", new List<Claim> { new Claim("Permission", "ManageProjects") })
            };

            foreach (var (roleName, description, claims) in rolesWithClaims)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole { Name = roleName, Description = description };
                    await _roleManager.CreateAsync(role);
                }

                // Přidání claimů k roli
                var roleToUpdate = await _roleManager.FindByNameAsync(roleName);
                foreach (var claim in claims)
                {
                    if (!(await _roleManager.GetClaimsAsync(roleToUpdate)).Any(c => c.Type == claim.Type && c.Value == claim.Value))
                    {
                        await _roleManager.AddClaimAsync(roleToUpdate, claim);
                    }
                }
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
                Console.WriteLine($"Chyba při odebrání role od uživatele: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při odebrání role od uživatele." });
            }
        }

        // Přidání claimu k uživateli
        public async Task<IdentityResult> AddClaimToUserAsync(string userId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Uživatel nebyl nalezen." });

            return await _userManager.AddClaimAsync(user, claim);
        }

        // Získání claimů uživatele
        public async Task<List<Claim>> GetClaimsForUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Uživatel nebyl nalezen.");

            return (await _userManager.GetClaimsAsync(user)).ToList();
        }

        // Přidání claimu k roli
        public async Task<IdentityResult> AddClaimToRoleAsync(string roleName, Claim claim)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' nebyla nalezena." });

            return await _roleManager.AddClaimAsync(role, claim);
        }

        // Získání claimů pro roli
        public async Task<List<Claim>> GetClaimsForRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException($"Role '{roleName}' nebyla nalezena.");

            return (await _roleManager.GetClaimsAsync(role)).ToList();
        }
    }
}
