namespace Aeternum.DTOs.User
{
    public class ApplicationUserRegisterDTO
    {
        // Křestní jméno uživatele
        public string FirstName { get; set; }

        // Příjmení uživatele
        public string LastName { get; set; }

        // Datum narození (volitelné)
        public DateTime? DateOfBirth { get; set; }

        // Email uživatele
        public string Email { get; set; }

        // Uživatelské jméno uživatele
        public string UserName { get; set; }

        // Heslo uživatele
        public string Password { get; set; }

        // Potvrzení hesla
        public string ConfirmPassword { get; set; }

        // Adresní informace (volitelné)
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
