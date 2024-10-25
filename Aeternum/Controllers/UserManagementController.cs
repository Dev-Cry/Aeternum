using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aeternum.DTOs.User;
using Aeternum.Services;
using System.Threading.Tasks;

namespace Aeternum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Všichni uživatelé musí být přihlášeni pro přístup k těmto metodám
    public class UserManagementController : ControllerBase
    {
        private readonly UserService _userService;

        public UserManagementController(UserService userService)
        {
            _userService = userService;
        }

        // Získání dat o uživateli
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Aktualizace profilu uživatele
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] ApplicationUserUpdateDTO updateDto)
        {
            var result = await _userService.UpdateUserAsync(userId, updateDto);
            if (result.Succeeded)
            {
                return Ok(new { message = "Profil uživatele byl úspěšně aktualizován." });
            }

            return BadRequest(result.Errors);
        }
    }
}
