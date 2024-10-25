using Aeternum.DTOs.User;
using Aeternum.Entities.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Aeternum.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        // Registrace uživatele
        public async Task<IdentityResult> RegisterAsync(ApplicationUserRegisterDTO registerDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }

        // Přihlášení uživatele
        public async Task<SignInResult> LoginAsync(ApplicationUserLoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);

            if (user == null)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
        }

        // Získání dat o uživateli
        public async Task<ApplicationUserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : _mapper.Map<ApplicationUserDTO>(user);
        }

        // Aktualizace profilu uživatele
        public async Task<IdentityResult> UpdateUserAsync(string userId, ApplicationUserUpdateDTO updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            _mapper.Map(updateDto, user);
            return await _userManager.UpdateAsync(user);
        }
    }
}
