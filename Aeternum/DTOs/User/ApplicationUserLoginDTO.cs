namespace Aeternum.DTOs.User
{
    public class ApplicationUserLoginDTO
    {
        // Uživatelské jméno nebo email
        public string UserNameOrEmail { get; set; }

        // Heslo uživatele
        public string Password { get; set; }
    }
}