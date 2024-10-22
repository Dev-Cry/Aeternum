using Microsoft.AspNetCore.Identity;

namespace Aeternum.Entities.User
{
    public class ApplicationUser : IdentityUser
    {
        // Křestní jméno uživatele (volitelné)
        public string? FirstName { get; set; }

        // Příjmení uživatele (volitelné)
        public string? LastName { get; set; }

        // Datum narození uživatele (volitelné)
        public DateTime? DateOfBirth { get; set; }

        // URL k profilovému obrázku uživatele (volitelné)
        public string? ProfilePictureUrl { get; set; }

        // Indikátor, zda je účet aktivní (výchozí hodnota je true, účet je aktivní)
        public bool IsActive { get; set; } = true;

        // Indikátor, zda je účet zablokován (výchozí hodnota je false, účet není blokován)
        public bool IsBlocked { get; set; } = false;

        // Datum, kdy byl účet zablokován (volitelné, může být null, pokud není blokován)
        public DateTime? BlockedAt { get; set; }

        // Datum, kdy byl účet vytvořen (automaticky nastaveno při registraci)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Datum poslední aktualizace profilu uživatele (automaticky nastaveno na aktuální datum a čas při vytvoření)
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Datum posledního přihlášení uživatele (volitelné, může být null, pokud se uživatel ještě nepřihlásil)
        public DateTime? LastLoginDate { get; set; }

        // Adresní informace

        // Ulice a číslo popisné (volitelné)
        public string? StreetAddress { get; set; }

        // Město (volitelné)
        public string? City { get; set; }

        // PSČ (volitelné)
        public string? PostalCode { get; set; }

        // Země (volitelné)
        public string? Country { get; set; }
    }
}
