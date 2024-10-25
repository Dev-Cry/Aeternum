namespace Aeternum.DTOs.User
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Adresní informace
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        // Přidání e-mailové a uživatelské jméno
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
