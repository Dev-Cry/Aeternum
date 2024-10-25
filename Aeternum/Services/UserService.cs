using Aeternum.DTOs.User;
using Aeternum.Entities.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    public async Task<IdentityResult> RegisterAsync(ApplicationUserRegisterDTO registerDto)
    {
        try
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }
        catch (Exception ex)
        {
            // Zpracování výjimky - např. logování
            // Můžete použít logging framework, jako je Serilog, NLog nebo log4net
            Console.WriteLine($"Chyba při registraci uživatele: {ex.Message}");
            return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při registraci uživatele." });
        }
    }

    public async Task<SignInResult> LoginAsync(ApplicationUserLoginDTO loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);

            if (user == null || user.IsBlocked)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
        }
        catch (Exception ex)
        {
            // Zpracování výjimky
            Console.WriteLine($"Chyba při přihlašování uživatele: {ex.Message}");
            return SignInResult.Failed;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            // Zpracování výjimky
            Console.WriteLine($"Chyba při odhlašování uživatele: {ex.Message}");
        }
    }

    public async Task<ApplicationUserDTO?> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : _mapper.Map<ApplicationUserDTO>(user);
        }
        catch (Exception ex)
        {
            // Zpracování výjimky
            Console.WriteLine($"Chyba při získávání uživatele podle ID: {ex.Message}");
            return null; // Můžete také vrátit nějaký objekt chyby
        }
    }

    public async Task<IdentityResult> UpdateUserAsync(string userId, ApplicationUserUpdateDTO updateDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Uživatel nebyl nalezen." });

            _mapper.Map(updateDto, user);
            return await _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            // Zpracování výjimky
            Console.WriteLine($"Chyba při aktualizaci uživatele: {ex.Message}");
            return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při aktualizaci uživatele." });
        }
    }

    // Přidání claimu uživateli
    public async Task<IdentityResult> AddClaimToUserAsync(string userId, Claim claim)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Uživatel nebyl nalezen." });

            return await _userManager.AddClaimAsync(user, claim);
        }
        catch (Exception ex)
        {
            // Zpracování výjimky
            Console.WriteLine($"Chyba při přidávání claimu uživateli: {ex.Message}");
            return IdentityResult.Failed(new IdentityError { Description = "Nastala chyba při přidávání claimu uživateli." });
        }
    }
}
