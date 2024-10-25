using Microsoft.AspNetCore.Mvc;
using Aeternum.DTOs.User;
using Aeternum.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Aeternum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthenticationController(UserService userService)
        {
            _userService = userService;
        }

        // Registrace uživatele
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterDTO registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            if (result.Succeeded)
            {
                return Ok(new { message = "Uživatel byl úspěšně registrován." });
            }

            return BadRequest(result.Errors);
        }

        // Přihlášení uživatele
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginDTO loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result.Succeeded)
            {
                return Ok(new { message = "Přihlášení bylo úspěšné." });
            }

            return Unauthorized(new { message = "Neplatné uživatelské jméno nebo heslo." });
        }

        // Odhlášení uživatele
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync(); // Přidejte tuto metodu do UserService
            return Ok(new { message = "Uživatel byl úspěšně odhlášen." });
        }
    }
}
