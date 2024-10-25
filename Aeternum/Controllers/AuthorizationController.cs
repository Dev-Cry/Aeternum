using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aeternum.Services;
using System.Threading.Tasks;

namespace Aeternum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public AuthorizationController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // Endpoint pro přiřazení role uživateli
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")] // Pouze pro adminy
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
        {
            var result = await _roleService.AddUserToRoleAsync(userId, roleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' byla přiřazena uživateli s ID '{userId}'." });
            }
            return BadRequest(result.Errors);
        }

        // Endpoint pro odebrání role uživateli
        [HttpPost("remove-role")]
        [Authorize(Roles = "Admin")] // Pouze pro adminy
        public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
        {
            var result = await _roleService.RemoveUserFromRoleAsync(userId, roleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' byla odebrána uživateli s ID '{userId}'." });
            }
            return BadRequest(result.Errors);
        }

        // Endpoint pro získání všech rolí
        [HttpGet("roles")]
        [Authorize] // Pro všechny autentizované uživatele
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}
